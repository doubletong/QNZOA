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
    public class ProjectBusinessesController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        public ProjectBusinessesController(SigbugsdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/ProjectBusinesses       
        [HttpGet("[action]")]
        public async Task<ProjectBusinessPagedVM> GetProjectBusinessesAsync(int page, int pageSize, string keywords)
        {
            var vm = new ProjectBusinessPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords
            };

            var query = _context.ProjectBusinesses.Include(d => d.Project).Include(d=>d.Paymentlogs).AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Project.Name.Contains(keywords));
            }

            vm.RowCount = await query.CountAsync();
            vm.TotalAmount = await query.SumAsync(d => d.Amount);
            vm.TotalPaymented = await query.SumAsync(d => d.Paymentlogs.Where(p => p.Money > 0).Sum(p => p.Money));
            vm.ProjectBusinesses = await query.OrderByDescending(d=>d.Project.CreatedDate)
                .Skip(vm.PageIndex * vm.PageSize)
                .Take(vm.PageSize)
                .ProjectTo<ProjectBusinessVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;

        }
        // GET: api/ProjectBusinesses/5
      
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProjectBusiness([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectBusiness = await _context.ProjectBusinesses.FindAsync(id);

            if (projectBusiness == null)
            {
                return NotFound();
            }

            return Ok(projectBusiness);
        }

        // PUT: api/ProjectBusinesses/5
      
        //[HttpPut("[action]/{id}")]
        //public async Task<IActionResult> PutProjectBusiness([FromRoute] int id, [FromBody] ProjectBusinessIM projectBusiness)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != projectBusiness.ProjectId)
        //    {
        //        return BadRequest();
        //    }

        //    var model = await _context.ProjectBusinesses.FirstOrDefaultAsync(d => d.ProjectId == id);
        //    model = _mapper.Map(projectBusiness, model);

        //    _context.Entry(model).State = EntityState.Modified; 

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProjectBusinessExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/ProjectBusinesses
        [HttpPost("[action]")]
        public async Task<IActionResult> PostProjectBusiness([FromBody] ProjectBusinessIM projectBusiness)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pb = await _context.ProjectBusinesses.FirstOrDefaultAsync(d => d.ProjectId == projectBusiness.ProjectId);
            if (pb != null)
            {
                pb = _mapper.Map(projectBusiness, pb);
                _context.Entry(pb).State = EntityState.Modified;
            }
            else
            {
                var model = _mapper.Map<ProjectBusiness>(projectBusiness);
                _context.ProjectBusinesses.Add(model);
            }

          
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProjectBusinessExists(projectBusiness.ProjectId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProjectBusiness", new { id = projectBusiness.ProjectId }, projectBusiness);
        }

        // DELETE: api/ProjectBusinesses/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProjectBusiness([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectBusiness = await _context.ProjectBusinesses.FindAsync(id);
            if (projectBusiness == null)
            {
                return NotFound();
            }

            _context.ProjectBusinesses.Remove(projectBusiness);
            await _context.SaveChangesAsync();

            return Ok(projectBusiness);
        }

        private bool ProjectBusinessExists(int id)
        {
            return _context.ProjectBusinesses.Any(e => e.ProjectId == id);
        }
    }
}