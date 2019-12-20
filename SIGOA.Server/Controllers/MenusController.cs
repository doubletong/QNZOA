using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGOA.Data;
using SIGOA.Model;

namespace SIGOA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class MenusController : ControllerBase
    {

        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MenusController(SigbugsdbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Menus
        [HttpGet("[action]")]
        [Authorize(Policy = "Permission")]
        public async Task<IEnumerable<MenuCategoryVM>> GetMenusAsync()
        {           
            var result = await _context.MenuCategories.Include(d => d.Menus).ProjectTo<MenuCategoryVM>(_mapper.ConfigurationProvider).ToListAsync();
            return result;
        }
        // GET: api/Menus
        [HttpGet("[action]/{categoryId}")]
        [Authorize(Policy = "Permission")]
        public async Task<List<MenuVM>> GetMenusByCategoryAsync([FromRoute]int categoryId)
        {
            var result = await _context.Menus.Where(d => d.CategoryId == categoryId).OrderBy(d=>d.Importance)
                .ProjectTo<MenuVM>(_mapper.ConfigurationProvider).ToListAsync();
            return result;
        }

        /// <summary>
        /// 获取我的菜单
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{categoryId}")]
        [Authorize]
        public async Task<List<MenuVM>> GetMyMenusByCategoryAsync([FromRoute]int categoryId)
        {
            var strUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;        
            var userId = new Guid(strUserId);
            var roles = await _context.Roles.Where(d=>d.UserRoles.Any(u=>u.UserId == userId)).Select(d=>d.Id).ToArrayAsync();
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            if (username == "admin")
            {             
                return await _context.Menus.Where(d => d.CategoryId == categoryId).OrderBy(d => d.Importance)
                        .ProjectTo<MenuVM>(_mapper.ConfigurationProvider).ToListAsync();
            }

            if (roles == null) return null;

            var result = await _context.Menus
                       .Where(d => d.CategoryId == categoryId && d.RoleMenus.Any(r=> roles.Contains( r.RoleId)))
                       .OrderBy(d => d.Importance)
                       .ProjectTo<MenuVM>(_mapper.ConfigurationProvider).ToListAsync();
            return result;

        }

        // GET: api/Menus/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetMenu([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var menu = await _context.Menus.FindAsync(id);

            if (menu == null)
            {
                return NotFound();
            }

            return Ok(menu);
        }
        // GET: api/Articles/5
        [HttpGet("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> EditMenu([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await _context.Menus.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            var im = _mapper.Map<MenuIM>(model);

            return Ok(im);
        }
        // PUT: api/Menus/5
        [HttpPut("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> PutMenu([FromRoute] int id, [FromBody] MenuIM menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != menu.Id)
            {
                return BadRequest();
            }
            var model = await _context.Menus.FindAsync(id);
            model = _mapper.Map(menu, model);
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
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



        // PUT: api/Menus/5
        [HttpPost("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> ExpandMenu([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                      
            var model = await _context.Menus.FindAsync(id);
            model.IsExpand = !model.IsExpand;
            _context.Entry(model).State = EntityState.Modified;

            await _context.SaveChangesAsync();
          

            return Ok();
        }

        // POST: api/Menus
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> PostMenu([FromBody] MenuIM menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var model = _mapper.Map<Menu>(menu);
            _context.Menus.Add(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

     
        [HttpDelete("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMenu([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();

            return Ok(menu);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> UpDownMoveAsync([FromBody] UpDownMoveIM im)
        {
            var vMenu = await _context.Menus.FindAsync(im.Id);
            var menuList = _context.Menus.Where(d => d.CategoryId == vMenu.CategoryId).OrderBy(m => m.Importance);

            if (im.IsUp)
            {
                                             
             

                var prevMenu = await _context.Menus
                    .OrderByDescending(d => d.Importance)
                    .FirstOrDefaultAsync(d => d.ParentId == vMenu.ParentId && d.Id != vMenu.Id && d.Importance <= vMenu.Importance);

                if (prevMenu == null)
                {
                    // 已经在第一位
                    return BadRequest("已经在第一位");
                }

                var num = prevMenu.Importance - vMenu.Importance;
                if (num == 0)
                {
                    vMenu.Importance = vMenu.Importance - 1;
                }
                else
                {
                    prevMenu.Importance = prevMenu.Importance - num;
                    vMenu.Importance = vMenu.Importance + num;
                }

                _context.Entry(prevMenu).State = EntityState.Modified;


                // ResetSort(vMenu.CategoryId);


            }
            else
            {
                               
                Menu nextMenu = await _context.Menus
                    .OrderBy(d => d.Importance).FirstOrDefaultAsync(d => d.ParentId == vMenu.ParentId && d.Id != vMenu.Id && d.Importance >= vMenu.Importance); 

                if (nextMenu == null)
                {
                    return BadRequest("已经在最后一位");                 
                }

                var num = nextMenu.Importance - vMenu.Importance;
                if (num == 0)
                {
                    vMenu.Importance = vMenu.Importance + 1;
                }
                else
                {
                    nextMenu.Importance = nextMenu.Importance - num;
                    vMenu.Importance = vMenu.Importance + num;
                }


                _context.Entry(nextMenu).State = EntityState.Modified;
              

            }

           
            _context.Entry(vMenu).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return Ok("菜单排位成功！");

            //// var menus = _menuService.GetLevelMenusByCategoryId(categoryId);
            //AR.Id = categoryId;
            //// AR.Data = await _viewRenderService.RenderAsync("_MenuList", menus);
            //var cacheKey = $"AllMenus_{categoryId}";
            //_cache.Remove(cacheKey);

            //AR.SetSuccess("菜单排位成功！");
            //return Json(AR);

            ////AR.Setfailure("菜单排位失败！");
            ////return Json(AR);

        }


        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> MoveMenuAsync([FromBody] MoveMenuIM im)
        {

            if (im.Id > 0 && im.ParentId > 0)
            {
                var parentMenu = await _context.Menus.FindAsync(im.ParentId);
                var menu = await _context.Menus.FindAsync(im.Id);
                menu.ParentId = im.ParentId;
                menu.LayoutLevel = parentMenu.LayoutLevel + 1;

                _context.Entry(menu).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok("移动菜单成功");

                //_menuService.ResetSort(menu.CategoryId);
                // var menus = _menuService.GetLevelMenusByCategoryId(menu.CategoryId);
                //AR.Id = menu.CategoryId;
                //// AR.Data = await _viewRenderService.RenderAsync("_MenuList", menus);
                //var cacheKey = $"AllMenus_{menu.CategoryId}";
                //_cache.Remove(cacheKey);
                //return Json(AR);

            }
            
            return BadRequest("移动菜单失败");
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.Id == id);
        }
    }
}