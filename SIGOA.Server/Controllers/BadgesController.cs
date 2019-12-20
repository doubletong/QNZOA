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
    public class BadgesController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        public BadgesController(SigbugsdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Badges
        [HttpGet("[action]")]
        public async Task<BadgePagedVM> GetBadgesAsync(int page, int pageSize, string keywords)
        {
            var vm = new BadgePagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords
            };

            var query = _context.Badges.AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Title.Contains(keywords));
            }

            vm.RowCount = await query.CountAsync();
            vm.ItemList = await query.OrderByDescending(d => d.Importance)
                .Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<BadgeVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Badges.OrderByDescending(d=>d.Importance)
                .ProjectTo<BadgeVM>(_mapper.ConfigurationProvider).ToListAsync(); 
            return Ok(list);
        }

        // GET: api/Badges/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetBadge([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var badge = await _context.Badges.FindAsync(id);

            if (badge == null)
            {
                return NotFound();
            }

            return Ok(badge);
        }

        // PUT: api/Badges/5
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutBadge([FromRoute] int id, [FromBody] BadgeIM badge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != badge.Id)
            {
                return BadRequest();
            }

            var model = await _context.Badges.FirstOrDefaultAsync(d => d.Id == id);
            model = _mapper.Map(badge, model);

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BadgeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "已成功修改" });
        }

        // POST: api/Badges
        [HttpPost("[action]")]
        public async Task<IActionResult> PostBadge([FromBody] BadgeIM badge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var model = _mapper.Map<Badge>(badge);
            _context.Badges.Add(model);
            await _context.SaveChangesAsync();


            return Ok(new { message = "任务标记创建成功" });
        }

        // DELETE: api/Badges/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteBadge([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var badge = await _context.Badges.FindAsync(id);
            if (badge == null)
            {
                return NotFound();
            }

            _context.Badges.Remove(badge);
            await _context.SaveChangesAsync();

            return Ok(new { message="任务标记已删除"});
        }

        private bool BadgeExists(int id)
        {
            return _context.Badges.Any(e => e.Id == id);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<SelectVM>), 200)]
        public async Task<List<SelectVM>> GetAllForSelectAsync()
        {

            return await _context.Badges.OrderByDescending(d => d.Id)
                .Select(d => new SelectVM
                {
                    Value = d.Id.ToString(),
                    Text = d.Title

                }).ToListAsync();

        }
    }
}