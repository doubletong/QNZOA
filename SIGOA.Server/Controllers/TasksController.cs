using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGOA.Data;
using SIGOA.Infrastructure;
using SIGOA.Infrastructure.Email;
using SIGOA.Model;

namespace SIGOA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TasksController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        public TasksController(SigbugsdbContext context, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }



        /// <summary>
        /// 获取所有项目分页任务
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="projectId"></param>
        /// <param name="keywords"></param>
        /// <param name="performer"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [Authorize(Policy = "Permission")]
        public async Task<TaskPagedVM> GetTasksAsync(int page, int pageSize, int projectId, string keywords,string performer, SIGOA.Data.Enums.TaskStatus status)
        {
            var vm = new TaskPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords,
                ProjectId = projectId,
                Status = status,
                Performer = performer
            };

            var query = _context.Tasks.Include(d => d.Project)
                .Include(d=>d.PerformerNavigation)
                .Include(d=>d.TaskBadges)
                .AsQueryable();
            if (vm.ProjectId > 0)
            {
                query = query.Where(d => d.ProjectId == vm.ProjectId);
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Title.Contains(keywords));
            }
            if (!string.IsNullOrEmpty(vm.Performer))
            {
                Guid userId = Guid.Empty;
                userId = new Guid(vm.Performer);
                query = query.Where(d => d.Performer == userId);
            }
            if (vm.Status>0)
            {
                query = query.Where(d => d.Status == status);
            }

            vm.RowCount = await query.CountAsync();
            vm.ItemList = await query.OrderByDescending(d => d.Id)
                .Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<TaskVM>(_mapper.ConfigurationProvider).ToListAsync();

            var badges = await _context.Badges.OrderByDescending(d=>d.Importance)
                .ProjectTo<BadgeVM>(_mapper.ConfigurationProvider).ToListAsync();

            foreach(var item in vm.ItemList)
            {
                item.Badges = badges.Where(d => item.BadgeIds.Contains(d.Id)).ToList();
            }

            return vm;
        }

        /// <summary>
        /// 获取我的项目任务
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="projectId"></param>
        /// <param name="keywords"></param>
        /// <param name="performer"></param>
        /// <param name="status"></param>
        /// <returns></returns>

        [HttpGet("[action]")]
        [Authorize]
        public async Task<TaskPagedVM> GetMyProjectTasksAsync(int page, int pageSize, int projectId, string keywords, SIGOA.Data.Enums.TaskStatus status)
        {
            var strUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            var userId = new Guid(strUserId);

            var vm = new TaskPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords,
                ProjectId = projectId,
                Status = status,
                Performer = strUserId
            };

            var query = _context.Tasks.Where(d => d.Performer == userId || d.Project.Manager == userId).Include(d => d.Project)
                .Include(d => d.PerformerNavigation)
                .Include(d => d.TaskBadges)
                .AsQueryable();
            if (vm.ProjectId > 0)
            {
                query = query.Where(d => d.ProjectId == vm.ProjectId);
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Title.Contains(keywords));
            }       
            if (vm.Status > 0)
            {
                query = query.Where(d => d.Status == status);
            }

            vm.RowCount = await query.CountAsync();
            vm.ItemList = await query.OrderByDescending(d => d.Id)
                .Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<TaskVM>(_mapper.ConfigurationProvider).ToListAsync();

            var badges = await _context.Badges.OrderByDescending(d => d.Importance)
                .ProjectTo<BadgeVM>(_mapper.ConfigurationProvider).ToListAsync();

            foreach (var item in vm.ItemList)
            {
                item.Badges = badges.Where(d => item.BadgeIds.Contains(d.Id)).ToList();
            }

            return vm;
        }



        // GET: api/Tasks/5
        [HttpGet("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> GetTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _context.Tasks.Include(d => d.Project)
                .Include(d => d.PerformerNavigation)
                .Include(d => d.TaskBadges).FirstOrDefaultAsync(d=>d.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<TaskDetailVM>(task);

            return Ok(model);
        }


        [HttpGet("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> EditTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _context.Tasks.Include(d=>d.TaskBadges).FirstOrDefaultAsync(d=>d.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            var im = _mapper.Map<TaskIM>(task);

            im.Badges = new List<SelectVM>();
            var badges = await _context.Badges.OrderByDescending(d => d.Importance).ToListAsync();

            var badgeIds = task.TaskBadges.Select(d => d.BadgeId);
            foreach(var b in badges)
            {
                im.Badges.Add(new SelectVM
                {
                    Value = b.Id.ToString(),
                    Text = b.Title,
                    Selected = badgeIds != null && badgeIds.Contains(b.Id)
                });
            }
                       
            return Ok(im);
        }

        // PUT: api/Tasks/5
        [HttpPut("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> PutTask([FromRoute] int id, [FromBody] TaskIM task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Id)
            {
                return BadRequest();
            }

            var model = await _context.Tasks.Include(d=>d.TaskBadges).FirstOrDefaultAsync(d => d.Id == id);
            model = _mapper.Map(task, model);
            model.TaskBadges.Clear();
            _context.Entry(model).State = EntityState.Modified;

            foreach(var item in task.Badges.Where(d=>d.Selected))
            {
                _context.TaskBadges.Add(new TaskBadge { TaskId = model.Id, BadgeId= int.Parse(item.Value) });
            }
      

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("已成功更新");
        }

        // POST: api/Tasks
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> PostTask([FromBody] TaskIM task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = _mapper.Map<SIGOA.Data.Task>(task);

    
            foreach (var item in task.Badges.Where(d => d.Selected))
            {
                model.TaskBadges.Add(new TaskBadge { TaskId = model.Id, BadgeId = int.Parse(item.Value) });
            }

            _context.Tasks.Add(model);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(model.Performer);
            if (Infrastructure.Helpers.Common.IsValidEmail(user.Email))
            {
                var emailAccount = await _context.EmailAccounts.FirstOrDefaultAsync(d => d.IsDefault);
                if (emailAccount != null)
                {
                    if (!string.IsNullOrEmpty(emailAccount.Password))
                    {
                        var pw = EncryptionHelper.Decrypt(emailAccount.Password);
                        emailAccount.Password = pw;
                    }
                    var subject = $"新任务：{model.Title}";
                    var body = $"{model.Body}<p><a href='http://oa.heiniaozhi.cn/myzone/mytasks/detail/{model.Id}'>去网站查看</a></p>";

                   
                    BackgroundJob.Enqueue(() => _emailService.SendMail("TZGOA", emailAccount.Email, user.Email, string.Empty,
                        subject, body, emailAccount.Smtpserver, emailAccount.Email, string.Empty, emailAccount.UserName,
                        emailAccount.Password, emailAccount.Port, emailAccount.EnableSsl));
                }
            }

            return Ok("已成功创建");
          
        }

        // POST: api/Tasks
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> SetTaskStatus([FromBody] SetTaskStatusIM im)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await _context.Tasks.FindAsync(im.Id);
            if (model == null)
            {
                return NotFound("未找到当前任务");
            }

            model.Status = im.Status;
            if(im.Status == Data.Enums.TaskStatus.Ongoing)
            {
                model.StartTime = DateTime.Now;
            }

            if (im.Status == Data.Enums.TaskStatus.Completed)
            {
                model.FinishTme = DateTime.Now;
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("已成功设置状态");


        }


        // DELETE: api/Tasks/5
        [HttpDelete("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound();
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return Ok("删除成功");
            }
            catch (Exception er)
            {
                return BadRequest(er.Message);
            }
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}