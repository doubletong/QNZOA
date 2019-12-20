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
   
    public class ProjectsController : ControllerBase
    {

        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        public ProjectsController(SigbugsdbContext context, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }


        // GET: api/Projects
        [HttpGet("[action]")]
        [Authorize(Policy = "Permission")]
        public async Task<ProjectPagedVM> GetProjectsAsync(int page, int pageSize, string keywords)
        {
            var vm = new ProjectPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords
            };

            var query = _context.Projects.Where(d=>d.Archive==false).Include(d=>d.Tasks).Include(d=>d.UserProjects)
                .Include(d=>d.ManagerNavigation).Include(d=>d.Customer).AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Name.Contains(keywords) || d.Description.Contains(keywords));
            }

            vm.RowCount = await query.CountAsync();
            vm.Projects = await query.OrderByDescending(d=>d.Id)
                .Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<ProjectVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
         
        }

        /// <summary>
        /// 我参与的项目
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [Authorize]
        public async Task<List<ProjectVM>> MyInProjectsAsync()
        {
            var strUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            var userId = new Guid(strUserId);         

            var projects = await _context.Projects.Where(d => d.UserProjects.Any(u=>u.UserId == userId))
                .OrderByDescending(d=>d.CreatedDate)
                .ProjectTo<ProjectVM>(_mapper.ConfigurationProvider).ToListAsync();
        
            return projects;
        }

        /// <summary>
        /// 我主管的项目
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [Authorize]
        public async Task<List<ProjectVM>> MyOnProjectsAsync()
        {
            var strUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            var userId = new Guid(strUserId);

            var projects = await _context.Projects.Where(d => d.Manager == userId)
                .OrderByDescending(d => d.CreatedDate)
                .ProjectTo<ProjectVM>(_mapper.ConfigurationProvider).ToListAsync();

            return projects;
        }


        // GET: api/Projects
        [HttpGet("[action]")]
        [Authorize(Policy = "Permission")]
        public async Task<ProjectPagedVM> GetArchiveProjectsAsync(int page, int pageSize, string keywords)
        {
            var vm = new ProjectPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords
            };

            var query = _context.Projects.Where(d => d.Archive).Include(d => d.Tasks).Include(d => d.UserProjects)
                .Include(d => d.ManagerNavigation).Include(d => d.Customer).AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Name.Contains(keywords) || d.Description.Contains(keywords));
            }

            vm.RowCount = await query.CountAsync();
            vm.Projects = await query.OrderByDescending(d => d.Id)
                .Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<ProjectVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;

        }


        // GET: api/Projects/5
        [HttpGet("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> GetProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }
            var vm = _mapper.Map<ProjectVM>(project);

            return Ok(vm);
        }

        // GET: api/Projects/5
        [HttpGet("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> GetProjectUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _context.Users
                .Where(d=>d.UserProjects.Any(c=>c.ProjectId == id))
                .ProjectTo<UserVM>(_mapper.ConfigurationProvider).ToListAsync();
  
            return Ok(users);
        }

        // DELETE: api/Projects/5
        [HttpDelete("[action]/{id}/{userId}")]
        [Authorize(Policy = "Permission")]
        public async Task<IActionResult> DeleteProjectUser([FromRoute] int id, [FromRoute] Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectUser = await _context.UserProjects.FirstOrDefaultAsync(d=>d.ProjectId == id && d.UserId == userId);
            if (projectUser == null)
            {
                return NotFound();
            }

            _context.UserProjects.Remove(projectUser);
            await _context.SaveChangesAsync();


            var users = await _context.Users
              .Where(d => d.UserProjects.Any(c => c.ProjectId == id))
              .ProjectTo<UserVM>(_mapper.ConfigurationProvider).ToListAsync();

            return Ok(users);
        }


        // POST: api/Projects
        [HttpPost("[action]")]
        [Authorize(Policy = "Permission")]
        public async Task<IActionResult> AddProjectUser([FromBody] ProjectUserIM im)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Guid userId = Guid.Empty;
            userId = new Guid(im.UserId);
            var model = await _context.UserProjects.FirstOrDefaultAsync(d=>d.UserId == userId && d.ProjectId == im.Id);
            if (model == null)
            {
                _context.UserProjects.Add(new UserProject { ProjectId = im.Id, UserId = userId});
                await _context.SaveChangesAsync();

                //发磅邮件通知
                var project = await _context.Projects.FindAsync(im.Id);
                var user = await _context.Users.FindAsync(userId);
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
                        var subject = $"邀请您加入项目：{project.Name}";
                        var body = $"<p>您已被邀请加入项目：{project.Name}</p>";


                        BackgroundJob.Enqueue(() => _emailService.SendMail("TZGOA", emailAccount.Email, user.Email, string.Empty,
                            subject, body, emailAccount.Smtpserver, emailAccount.Email, string.Empty, emailAccount.UserName,
                            emailAccount.Password, emailAccount.Port, emailAccount.EnableSsl));
                    }
                }


                return Ok("已成功添加");
            }
            else
            {
                return Ok("已存在此人员");
            }
         
        }



        [HttpGet("[action]/{id}")]
        [Authorize(Policy = "Permission")]
        public async Task<IActionResult> EditProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            var im = _mapper.Map<ProjectIM>(project);

            return Ok(im);
        }

        // PUT: api/Projects/5
        [HttpPut("[action]/{id}")]
        [Authorize(Policy = "Permission")]
        public async Task<IActionResult> PutProject([FromRoute] int id, [FromBody] ProjectIM project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.Id)
            {
                return BadRequest();
            }

            var model = await _context.Projects.FirstOrDefaultAsync(d => d.Id == id);
            model = _mapper.Map(project, model);

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

       /// <summary>
       /// 添加项目
       /// </summary>
       /// <param name="project"></param>
       /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize(Policy = "Permission")]
        public async Task<IActionResult> PostProject([FromBody] ProjectIM project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var  model = _mapper.Map<Project>(project);
            model.Archive = false;

            _context.Projects.Add(model);
            await _context.SaveChangesAsync();

            return Ok("已成功创建");
        }

        /// <summary>
        /// 项目归档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]/{id}")]
        [Authorize(Policy = "Permission")]
        public async Task<IActionResult> ArchiveProject([FromRoute] int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            project.Archive = true;
            project.ArchiveDate = DateTime.Now;
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("已归档");
        }

        /// <summary>
        /// 取消项目归档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]/{id}")]
        [Authorize(Policy = "Permission")]
        public async Task<IActionResult> UnArchiveProject([FromRoute] int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            project.Archive = false;
            project.ArchiveDate = DateTime.Now;
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("已解档");
        }

        // DELETE: api/Projects/5
        [HttpDelete("[action]/{id}")]
        [Authorize(Policy = "Permission")]
        public async Task<IActionResult> DeleteProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }


        [HttpGet("[action]"), AllowAnonymous]
        [ProducesResponseType(typeof(List<SelectVM>), 200)]
        public async Task<List<SelectVM>> GetAllForSelectAsync()
        {

            return await _context.Projects.OrderByDescending(d => d.Id)
                .Select(d => new SelectVM
                {
                    Value = d.Id.ToString(),
                    Text = d.Name

                }).ToListAsync();

        }

        [HttpGet("[action]"), AllowAnonymous]
        [ProducesResponseType(typeof(List<SelectVM>), 200)]
        public async Task<List<SelectVM>> GetMyAllForSelectAsync()
        {
            var strUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            var userId = new Guid(strUserId);

            return await _context.Projects.Where(d=>d.Manager == userId || d.UserProjects.Any(p=>p.UserId == userId)).OrderByDescending(d => d.Id)
                .Select(d => new SelectVM
                {
                    Value = d.Id.ToString(),
                    Text = d.Name

                }).ToListAsync();

        }


        [HttpGet("[action]"), AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ProjectForSelectVM>), 200)]
        public async Task<IEnumerable<ProjectForSelectVM>> GetWithPricingAsync()
        {

            return await _context.ProjectBusinesses.Include(d=>d.Project)
                .OrderByDescending(d => d.ProjectId)
                .Select(d=>new ProjectForSelectVM { Id= d.ProjectId,Name = d.Project.Name })
                .ToListAsync();

        }
       
        [HttpGet("[action]"), AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ProjectForSelectVM>), 200)]
        public async Task<IEnumerable<ProjectForSelectVM>> GetWithBusinessesAsync()
        {

            return await _context.Projects.Where(d=>d.ProjectBusiness==null)
                .OrderByDescending(d => d.Id)
                .Select(d => new ProjectForSelectVM { Id = d.Id, Name = d.Name })
                .ToListAsync();

        }
    }
}