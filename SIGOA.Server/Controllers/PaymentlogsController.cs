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
    //[Authorize]
    public class PaymentlogsController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentlogsController(SigbugsdbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        #region for manager
        /// <summary>
        /// 获取分页流水帐
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(PaymentlogPagedVM), 200)]
        [Authorize(Policy = "Permission")]
      
        public async Task<PaymentlogPagedVM> GetPaymentlogsAsync(int page, int pageSize, int projectId)
        {
            var vm = new PaymentlogPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                ProjectId = projectId
            };

            var query = _context.Paymentlogs.Include(d=>d.Project).AsQueryable();
            if (projectId>0)
            {
                query = query.Where(d => d.ProjectId==projectId);
            }

            vm.RowCount = await query.CountAsync();
            vm.Paymentlogs = await query.OrderByDescending(d=>d.Id).Skip(vm.PageIndex * vm.PageSize)
                .Take(vm.PageSize)
                .ProjectTo<PaymentlogVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
       
        }


        [HttpGet("[action]")]
        [ProducesResponseType(typeof(PaymentlogPagedVM), 200)]
        [Authorize]

        public async Task<PaymentlogPagedVM> GetMyPaymentlogsAsync(int projectId)
        {
            var vm = new PaymentlogPagedVM
            {             
                ProjectId = projectId
            };

            var strUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            var userId = new Guid(strUserId);

            var query = _context.Paymentlogs.Where(d=>d.Project.UserProjects.Any(p=>p.UserId == userId) || d.Project.Manager == userId).Include(d => d.Project).AsQueryable();
            if (projectId > 0)
            {
                query = query.Where(d => d.ProjectId == projectId);
            }

            vm.RowCount = await query.CountAsync();
            vm.Paymentlogs = await query.OrderByDescending(d => d.Id).ProjectTo<PaymentlogVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;

        }

        /// <summary>
        /// 单条详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> GetPaymentlog([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentlog = await _context.Paymentlogs.FindAsync(id);

            if (paymentlog == null)
            {
                return NotFound();
            }

            return Ok(paymentlog);
        }


        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutPaymentlog([FromRoute] int id, [FromBody] PaymentlogIM paymentlog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != paymentlog.Id)
            {
                return BadRequest();
            }

            if(paymentlog.PayType == 2)
            {
                paymentlog.Money = -paymentlog.Money;
            }

            var model = await _context.Paymentlogs.FirstOrDefaultAsync(d => d.Id == id);
            model = _mapper.Map(paymentlog, model);

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// 新建流水帐
        /// </summary>
        /// <param name="paymentlog"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> PostPaymentlog([FromBody] PaymentlogIM paymentlog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (paymentlog.PayType == 2)
            {
                paymentlog.Money = -paymentlog.Money;
            }

            var vm = _mapper.Map<Paymentlog>(paymentlog);

            _context.Paymentlogs.Add(vm);
            await _context.SaveChangesAsync();

            return Ok(vm);
        }

        // DELETE: api/Paymentlogs/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeletePaymentlog([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentlog = await _context.Paymentlogs.FindAsync(id);
            if (paymentlog == null)
            {
                return NotFound();
            }

            _context.Paymentlogs.Remove(paymentlog);
            await _context.SaveChangesAsync();

            return Ok(paymentlog);
        }

        private bool PaymentlogExists(int id)
        {
            return _context.Paymentlogs.Any(e => e.Id == id);
        }

        #endregion

        #region for APP

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(PaymentlogPagedVM), 200)]
        public async Task<PaymentlogPagedVM> MyProjectPaymentlogsAsync(int page, int pageSize, int projectId)
        {
            var strUserId = "13d818fd-687c-c44c-8413-08d522565bcc";  //_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            var userId = new Guid(strUserId);

            var vm = new PaymentlogPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                ProjectId = projectId
            };

            var query = _context.Paymentlogs.Include(d => d.Project)
                .Where(d=>d.Project.UserProjects.Any(c=>c.UserId == userId)).AsQueryable();
            if (projectId > 0)
            {
                query = query.Where(d => d.ProjectId == projectId);
            }

            vm.RowCount = await query.CountAsync();
            vm.Paymentlogs = await query.OrderByDescending(d => d.Id).Skip(vm.PageIndex * vm.PageSize)
                .Take(vm.PageSize)
                .ProjectTo<PaymentlogVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;

        }

        #endregion
    }
}