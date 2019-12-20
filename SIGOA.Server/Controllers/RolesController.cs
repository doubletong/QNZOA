using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGOA.Data;
using SIGOA.Model;
using SIGOA.Infrastructure.Extensions;

namespace SIGOA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        public RolesController(SigbugsdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

   
        [HttpGet("[action]")]
        public async Task<List<RoleVM>> GetRolesAsync()
        {
            return await _context.Roles.Include(d=>d.UserRoles)
                .ProjectTo<RoleVM>(_mapper.ConfigurationProvider).ToListAsync();
        }

   
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetRole([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> EditRole([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var im = _mapper.Map<RoleIM>(role);

            return Ok(im);
        }

        // PUT: api/Roles/5
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutRole([FromRoute] int id, [FromBody] RoleIM role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != role.Id)
            {
                return BadRequest();
            }

            var model = await _context.Roles.FirstOrDefaultAsync(d => d.Id == id);
            model = _mapper.Map(role, model);          

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Roles
        [HttpPost("[action]")]
        public async Task<IActionResult> PostRole([FromBody] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var model = _mapper.Map<Role>(role);
            _context.Roles.Add(model);
            await _context.SaveChangesAsync();         

            return Ok();
        }


        [HttpGet("[action]/{id}")]       
        public async Task<ActionResult> SetRoleMenusAsync(int id)
        {
            var role = await _context.Roles.Include(d=>d.RoleMenus).FirstOrDefaultAsync(d=>d.Id ==id);
            var menuIds = role.RoleMenus.Select(m => m.MenuId).ToList();
            //var menus = _.GetMenusByCategoryId(SettingsManager.Menu.BackMenuCId);
            var menus = await _context.Menus.Where(d=>d.CategoryId == 1).ProjectTo<RoleMenusIM>(_mapper.ConfigurationProvider).ToListAsync();
            foreach(var menu in menus)
            {
                menu.IsSelected = menuIds.Any()? menuIds.Contains(menu.Id):false;               
            }            
                  
            SetRoleMenusIM vm = new SetRoleMenusIM
            {
                RoleId = id,
                Menus = menus
            };

            return Ok(vm);
        }

        //private void LoadSubMenus(Menu menu, List<int> menuIds)
        //{
        //   _context.Entry(menu).Collection(u => u.InverseParent).Load();
        //    foreach (var subMenu in menu.InverseParent)
        //    {
        //        subMenu.IsSelected = menuIds.Contains(subMenu.Id);
        //        this.LoadSubMenus(subMenu, menuIds);
        //    }
        //}



        [HttpPost("[action]")]
        public async Task<ActionResult> SetRoleMenusAsync([FromBody] SetRoleMenusIM im)
        {

            var rolemenus = await _context.RoleMenus.Where(d => d.RoleId == im.RoleId).ToListAsync();

            _context.RoleMenus.RemoveRange(rolemenus);
            await _context.SaveChangesAsync();

            
            foreach (var m in im.Menus)
            {
                SetRoleMenusPer(m,im.RoleId);
            }

            await _context.SaveChangesAsync();
      
            return Ok("角色权限设置成功");
        }

        private void SetRoleMenusPer(RoleMenusIM menu,int roleId)
        {
            if (menu.IsSelected)
            {
                _context.RoleMenus.Add(new RoleMenu { RoleId = roleId, MenuId = menu.Id });
            }
            if (menu.InverseParent.Any())
            {
                foreach (var subMenu in menu.InverseParent)
                {
                    this.SetRoleMenusPer(subMenu, roleId);
                }
            }           
        }


        // DELETE: api/Roles/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok(role);
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}