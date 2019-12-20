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
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        public CustomersController(SigbugsdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

     
        /// <summary>
        /// 获取分页客户
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(CustomerPagedVM),200)]
        public async Task<CustomerPagedVM> GetCustomersAsync(int page,int pageSize,string keywords)
        {
            var vm = new CustomerPagedVM
            {
                PageIndex = page-1,
                PageSize = pageSize,
                Keywords = keywords
            };

            var query = _context.Customers.Include(d=>d.Projects).AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Name.Contains(keywords) || d.Description.Contains(keywords) || d.Phone.Contains(keywords));
            }

            vm.RowCount = await query.CountAsync();
            vm.Customers = await query.OrderByDescending(d=>d.CreatedDate)
                .Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<CustomerVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            var vm = _mapper.Map<CustomerVM>(customer);
            return Ok(customer);
        }

        /// <summary>
        /// 编辑客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> EditCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var im = _mapper.Map<CustomerIM>(customer);

            return Ok(im);
        }


        // PUT: api/Customers/5
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] CustomerIM customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest("不是有效的Id");
            }

            var model = await _context.Customers.FirstOrDefaultAsync(d => d.Id == id); 

            model = _mapper.Map(customer, model);

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        [HttpPost("[action]")]
        public async Task<IActionResult> PostCustomer([FromBody] CustomerIM customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var vm = _mapper.Map<Customer>(customer);

            _context.Customers.Add(vm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IEnumerable<CustomerForSelectVM>), 200)]
        public async Task<IEnumerable<CustomerForSelectVM>> GetAllForSelectAsync()
        {
            
            return await _context.Customers.OrderByDescending(d=>d.Id)
                .ProjectTo<CustomerForSelectVM>(_mapper.ConfigurationProvider).ToListAsync();
            
        }
    }
}