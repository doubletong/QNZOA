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
    public class ArticlesController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        public ArticlesController(SigbugsdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/Articles
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ArticlePagedVM), 200)]
        public async Task<ArticlePagedVM> GetArticlesAsync(int page, int pageSize,int categoryId, string keywords)
        {
            var vm = new ArticlePagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords,
                CategoryId = categoryId
            };

            var query = _context.Articles.Include(d => d.Category).AsQueryable();
            if (vm.CategoryId > 0)
            {
                query = query.Where(d => d.CategoryId == vm.CategoryId);
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Title.Contains(keywords) || d.Summary.Contains(keywords));
            }

            vm.RowCount = await query.CountAsync();
            vm.ItemList = await query.OrderByDescending(d => d.CreatedDate)
                .Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<ArticleVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
        }

        // GET: api/Articles/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetArticle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = await _context.Articles.Include(d=>d.Category).FirstOrDefaultAsync(d=>d.Id == id);

            if (article == null)
            {
                return NotFound();
            }
            article.Viewcount++;
            _context.Entry(article).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var model = _mapper.Map<ArticleDetailVM>(article);
            return Ok(model);
        }

        // GET: api/Articles/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> EditArticle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            var im = _mapper.Map<ArticleIM>(article);

            return Ok(im);
        }

        // PUT: api/Articles/5
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutArticle([FromRoute] int id, [FromBody] ArticleIM article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != article.Id)
            {
                return BadRequest();
            }

            var model = await _context.Articles.FirstOrDefaultAsync(d => d.Id == id);
            model = _mapper.Map(article, model);

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
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



        // POST: api/Articles
        [HttpPost("[action]")]
        public async Task<IActionResult> PostArticle([FromBody] ArticleIM article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var model = _mapper.Map<Article>(article);
            _context.Articles.Add(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteArticle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return Ok(article);
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}