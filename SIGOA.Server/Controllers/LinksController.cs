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
    public class LinksController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        public LinksController(SigbugsdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<LinkPagedVM> GetLinksAsync(int page, int pageSize, int categoryId, string keywords)
        {
            var vm = new LinkPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords,
                CategoryId = categoryId
            };

            var query = _context.Links.Include(d => d.Category).AsQueryable();
            if (vm.CategoryId > 0)
            {
                query = query.Where(d => d.CategoryId == vm.CategoryId);
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Title.Contains(keywords));
            }

            vm.RowCount = await query.CountAsync();
            vm.ItemList = await query.OrderByDescending(d => d.Importance).ThenByDescending(d=>d.CreatedDate)
                .Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<LinkVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetLink([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var link = await _context.Links.FindAsync(id);

            if (link == null)
            {
                return NotFound();
            }

            return Ok(link);
        }

        // GET: api/Articles/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> EditLink([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _context.Links.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            var im = _mapper.Map<LinkIM>(item);

            return Ok(im);
        }
    
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutLink([FromRoute] int id, [FromBody] LinkIM link)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != link.Id)
            {
                return BadRequest();
            }

            var model = await _context.Links.FirstOrDefaultAsync(d => d.Id == id);
            model = _mapper.Map(link, model);

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinkExists(id))
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


        [HttpPost("[action]")]
        public async Task<IActionResult> PostLink([FromBody] LinkIM link)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = _mapper.Map<Link>(link);
            _context.Links.Add(model);
            await _context.SaveChangesAsync();


            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteLink([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var link = await _context.Links.FindAsync(id);
            if (link == null)
            {
                return NotFound();
            }

            _context.Links.Remove(link);
            await _context.SaveChangesAsync();

            return Ok(link);
        }

        private bool LinkExists(int id)
        {
            return _context.Links.Any(e => e.Id == id);
        }
    }
}