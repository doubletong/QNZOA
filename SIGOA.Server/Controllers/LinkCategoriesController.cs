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

namespace SIGOA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkCategoriesController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        public LinkCategoriesController(SigbugsdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ArticleCategoryPagedVM), 200)]
        public async Task<LinkCategoryPagedVM> GetLinkCategoriesAsync(int page, int pageSize, string keywords)
        {
            var vm = new LinkCategoryPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords
            };

            var query = _context.LinkCategories.AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Title.Contains(keywords) );
            }

            vm.RowCount = await query.CountAsync();
            vm.ItemList = await query.OrderByDescending(d => d.Importance).ThenByDescending(d=>d.CreatedDate)
                .Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<LinkCategoryVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetLinkCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var linkCategory = await _context.LinkCategories.FindAsync(id);

            if (linkCategory == null)
            {
                return NotFound();
            }

            return Ok(linkCategory);
        }

        [HttpPut("[action]/{id}")]       
        public async Task<IActionResult> PutLinkCategory([FromRoute] int id, [FromBody] LinkCategoryIM linkCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != linkCategory.Id)
            {
                return BadRequest();
            }

            var model = await _context.LinkCategories.FirstOrDefaultAsync(d => d.Id == id);
            model = _mapper.Map(linkCategory, model);

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinkCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("链接分类更新成功");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PostLinkCategory([FromBody] LinkCategoryIM linkCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = _mapper.Map<LinkCategory>(linkCategory);
            _context.LinkCategories.Add(model);
            await _context.SaveChangesAsync();


            return Ok("链接分类创建成功");
        }



        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteLinkCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var linkCategory = await _context.LinkCategories.FindAsync(id);
            if (linkCategory == null)
            {
                return NotFound();
            }

            _context.LinkCategories.Remove(linkCategory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool LinkCategoryExists(int id)
        {
            return _context.LinkCategories.Any(e => e.Id == id);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<SelectVM>), 200)]
        public async Task<List<SelectVM>> GetAllForSelectAsync()
        {

            return await _context.LinkCategories.OrderByDescending(d => d.Importance)
                .Select(d => new SelectVM
                {
                    Value = d.Id.ToString(),
                    Text = d.Title

                }).ToListAsync();

        }
    }
}