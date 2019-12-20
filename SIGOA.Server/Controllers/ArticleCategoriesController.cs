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
    public class ArticleCategoriesController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        public ArticleCategoriesController(SigbugsdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ArticleCategories
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ArticleCategoryPagedVM), 200)]
        public async Task<ArticleCategoryPagedVM> GetArticleCategoriesAsync(int page, int pageSize, string keywords)
        {
            var vm = new ArticleCategoryPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords
            };

            var query = _context.ArticleCategories.AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Title.Contains(keywords) || d.Description.Contains(keywords));
            }

            vm.RowCount = await query.CountAsync();
            vm.ItemList = await query.OrderByDescending(d => d.CreatedDate)
                .Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<ArticleCategoryVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetArticleCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var articleCategory = await _context.ArticleCategories.FindAsync(id);

            if (articleCategory == null)
            {
                return NotFound();
            }

            return Ok(articleCategory);
        }


        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutArticleCategory([FromRoute] int id, [FromBody] ArticleCategoryIM articleCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != articleCategory.Id)
            {
                return BadRequest();
            }

            var model = await _context.ArticleCategories.FirstOrDefaultAsync(d => d.Id == id);
            model = _mapper.Map(articleCategory, model);

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleCategoryExists(id))
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

        // POST: api/ArticleCategories
        [HttpPost("[action]")]
        public async Task<IActionResult> PostArticleCategory([FromBody] ArticleCategoryIM articleCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = _mapper.Map<ArticleCategory>(articleCategory);
            _context.ArticleCategories.Add(model);
            await _context.SaveChangesAsync();

            return Ok();  // CreatedAtAction("GetArticleCategory", new { id = articleCategory.Id }, articleCategory);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteArticleCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var articleCategory = await _context.ArticleCategories.FindAsync(id);
            if (articleCategory == null)
            {
                return NotFound();
            }

            _context.ArticleCategories.Remove(articleCategory);
            await _context.SaveChangesAsync();

            return Ok(articleCategory);
        }

        private bool ArticleCategoryExists(int id)
        {
            return _context.ArticleCategories.Any(e => e.Id == id);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<SelectVM>), 200)]
        public async Task<List<SelectVM>> GetAllForSelectAsync()
        {

            return await _context.ArticleCategories.OrderByDescending(d => d.Importance)
                .Select(d=>new SelectVM
                {
                    Value = d.Id.ToString(),
                    Text = d.Title
                
                }).ToListAsync();

        }
    }
}