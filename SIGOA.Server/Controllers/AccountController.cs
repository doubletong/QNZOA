using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SIGOA.Data;
using SIGOA.Infrastructure;
using SIGOA.Model;

namespace SIGOA.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly string _issuer;
        private readonly string _key;
        public AccountController(SigbugsdbContext context,
            IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _context = context;         
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _issuer = config.GetValue<string>("TokenOptions:Issuer");
            _key = config.GetValue<string>("TokenOptions:Key");
        }
       
        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Token([FromBody] LoginIM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(d => d.Username == model.Username);
            var salt = Convert.FromBase64String(user.SecurityStamp);
            var pwdHash = EncryptionHelper.HashPasswordWithSalt(model.Password, salt);

            if (user.PasswordHash != pwdHash)
            {
                return BadRequest(new ReturnVM { Message = "用户名或密码不正确。" });
            }

            var token = await GetJwtSecurityTokenAsync(user);
            var vm = new UserLoginVM
            {
                Id = user.Id,
                Username = user.Username,
                RealName = user.RealName,
                Email = user.Email,
                PhotoUrl = user.PhotoUrl,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            return Ok(vm);
        }

        [HttpPost("[action]")]
        [AllowAnonymous,Obsolete]
        public async Task<IActionResult> Authenticate(string Username,string Password)
        {
         

            var user = await _context.Users.FirstOrDefaultAsync(d => d.Username == Username);
            var salt = Convert.FromBase64String(user.SecurityStamp);
            var pwdHash = EncryptionHelper.HashPasswordWithSalt(Password, salt);

            if (user.PasswordHash != pwdHash)
            {
                return BadRequest("用户名或密码不正确。");
            }

            var token = await GetJwtSecurityTokenAsync(user);
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
        private async Task<JwtSecurityToken> GetJwtSecurityTokenAsync(User user)
        {
            // var userClaims = await _userManager.GetClaimsAsync(user);
            // create claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim("RealName", user.RealName??"无"),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var userRoles = await _context.Roles.Where(d => d.UserRoles.Any(ur => ur.UserId == user.Id)).ToArrayAsync();
            //_roleServices.GetRolesByUserId(user.Id).ToArray();
            //add a list of roles

            if (userRoles.Any())
            {
                var roles = string.Join(",", userRoles.Select(d => d.RoleName));
                claims.Add(new Claim(ClaimTypes.Role, roles));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _issuer,
                audience: _issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: creds
            );
        }

        // GET: api/Users/5
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var strUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            var userId = new Guid(strUserId);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            var vm = _mapper.Map<UserIM>(user);
            return Ok(vm);
        }

        // PUT: api/Users/5
        [HttpPut("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromRoute] Guid id, [FromBody] UserIM user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest("资料修改失败");
            }

            var strUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            var userId = new Guid(strUserId);
            if (userId != user.Id)
            {
                return BadRequest("不是当前用户");
            }

            if (_context.Users.Where(d => d.Email == user.Email && d.Id != id).Any())
            {
                return BadRequest("此邮箱已被注册！");
            }

            var model = await _context.Users.FirstOrDefaultAsync(d => d.Id == id);
            model = _mapper.Map(user, model);
            if (model.Birthday == DateTime.MinValue)
            {
                model.Birthday = null;
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();        

            return Ok("资料已成功修改");
        }
    }
}