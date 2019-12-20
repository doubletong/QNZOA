using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGOA.Data;
using SIGOA.Infrastructure;
using SIGOA.Model;

namespace SIGOA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        public UsersController(SigbugsdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


      
        [HttpGet("[action]")]
        public async Task<UserPagedVM> GetUsersAsync(int page, int pageSize, string keywords)
        {
            var vm = new UserPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords
            };

            var query = _context.Users.Include(d => d.UserProjects)
               .AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Username.Contains(keywords) || d.RealName.Contains(keywords));
            }

            vm.RowCount = await query.CountAsync();
            vm.ItemList = await query.Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<UserVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;

        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> EditUser([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var im = _mapper.Map<UserIM>(user);

            return Ok(im);
        }
        // PUT: api/Users/5
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutUser([FromRoute] Guid id, [FromBody] UserIM user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            if(_context.Users.Where(d=>d.Email == user.Email && d.Id != id).Any())
            {
                return BadRequest("此邮箱已被注册！");
            }

            var model = await _context.Users.FirstOrDefaultAsync(d => d.Id == id);
            model = _mapper.Map(user, model);
            if(model.Birthday == DateTime.MinValue)
            {
                model.Birthday = null;
            }

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("用户资料已成功修改");
        }

        [HttpPost("[action]")]
 
        public async Task<IActionResult> CreateUserAsync([FromBody]RegisterIM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _context.Users.CountAsync(d => d.Email == model.Email);

            if (result>0)
            {
                return BadRequest("邮箱已存在"); 
            }

            result = await _context.Users.CountAsync(d => d.Username == model.UserName);

            if (result>0)
            {
                return BadRequest("用户名已存在"); 
            }
                     

            var securityStamp = EncryptionHelper.GenerateSalt();
            var passwordHash = EncryptionHelper.HashPasswordWithSalt(model.Password, securityStamp);

            var newUser = new User()
            {
                Id = IdentityGenerator.SequentialGuid(),
                Username = model.UserName,            
                Email = model.Email,            
                SecurityStamp = Convert.ToBase64String(securityStamp),
                PasswordHash = passwordHash,
                CreateDate = DateTime.Now,
                IsActive = true,
               
            };

            _context.Add(newUser);      
            await _context.SaveChangesAsync();
        
            return Ok("用户创建成功");
        }

        [HttpPost("[action]")]     
        public async Task<IActionResult> SetPassword(SetPasswordIM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(model.UserId);
  
            var securityStamp = EncryptionHelper.GenerateSalt();
            var pwdHash = EncryptionHelper.HashPasswordWithSalt(model.NewPassword, securityStamp);

            user.SecurityStamp = Convert.ToBase64String(securityStamp);
            user.PasswordHash = pwdHash;
            _context.Update(user);
            var result = await _context.SaveChangesAsync();


            if (result > 0)
            {               
                return Ok("密码已经成功重设");
            }
           
            return BadRequest("密码重设失败");

        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUserRoles([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roles = await _context.Roles.ToListAsync();

            var ids = await _context.Roles.Where(d=>d.UserRoles.Any(u=>u.UserId == id))
                .Select(d=>d.Id).ToListAsync();

            var vm = new SetRolesIM
            {
                UserId = id,
                Roles = new List<SelectVM>()
            };
            foreach(var r in roles)
            {
                vm.Roles.Add(new SelectVM
                {
                    Value = r.Id.ToString(),
                    Text = r.RoleName,
                    Selected = ids != null && ids.Contains(r.Id)
                });
            }

            return Ok(vm);
        }

        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> SetRoleAsync([FromBody] SetRolesIM model)
        {
            try
            {
                var userRoles =  await _context.UserRoles.Where(d => d.UserId == model.UserId).ToListAsync();
                _context.RemoveRange(userRoles);

                

                foreach (var item in model.Roles.Where(d=>d.Selected))
                {
                    _context.UserRoles.Add(new UserRole { UserId = model.UserId, RoleId = int.Parse(item.Value) });
                }
                await _context.SaveChangesAsync();

                return Ok("角色设置成功");
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FirstOrDefaultAsync(d=>d.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _context.Projects.CountAsync(d => d.Manager == id);
            if (result>0)
            {
                return BadRequest($"此帐号下管理着{result}个项目，不可删除。");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


        /// <summary>
        /// 用户列表下拉
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("[action]"),ProducesResponseType(typeof(IEnumerable<UserForSelectVM>), 200)]
        public async Task<IEnumerable<UserForSelectVM>> GetAllForSelectAsync()
        {

            //return await _context.Users.OrderBy(d => d.CreateDate)
            //    .ProjectTo<UserForSelectVM>(_mapper.ConfigurationProvider).ToListAsync();
            return await _context.Users.OrderBy(d => d.CreateDate)
                .Select(d=>new UserForSelectVM
                {
                    Id = d.Id,
                    Username = d.Username,
                    RealName = d.RealName,
                    IsActive = d.IsActive
                }).ToListAsync();

        }
    }
}