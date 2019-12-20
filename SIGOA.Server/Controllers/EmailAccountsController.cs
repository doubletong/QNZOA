using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGOA.Data;
using SIGOA.Infrastructure;
using SIGOA.Infrastructure.Email;
using SIGOA.Model;

namespace SIGOA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailAccountsController : ControllerBase
    {      

        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        public EmailAccountsController(SigbugsdbContext context, IMapper mapper, 
            IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }


        // GET: api/GetEmailAccountsAsync
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(EmailAccountPagedVM), 200)]
        public async Task<EmailAccountPagedVM> GetEmailAccountsAsync(int page, int pageSize, string keywords)
        {
            var vm = new EmailAccountPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords
            };

            var query = _context.EmailAccounts.AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Email.Contains(keywords) || d.UserName.Contains(keywords));
            }

            vm.RowCount = await query.CountAsync();
            vm.ItemList = await query.OrderByDescending(d => d.CreatedDate)
                .Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<EmailAccountVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
        }

        // GET: api/EmailAccounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmailAccount([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailAccount = await _context.EmailAccounts.FindAsync(id);

            if (emailAccount == null)
            {
                return NotFound();
            }

            return Ok(emailAccount);
        }

        // PUT: api/EmailAccounts/5
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutEmailAccount([FromRoute] int id, [FromBody] EmailAccountIM emailAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emailAccount.Id)
            {
                return BadRequest();
            }

            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;         

            var model = await _context.EmailAccounts.FirstOrDefaultAsync(d => d.Id == id);
            model = _mapper.Map(emailAccount, model);

            model.Password = EncryptionHelper.Encrypt(model.Password);
            model.UpdatedDate = DateTime.Now;
            model.UpdatedBy = userName;

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailAccountExists(id))
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

        // POST: api/EmailAccounts
        [HttpPost("[action]")]
        public async Task<IActionResult> PostEmailAccount([FromBody] EmailAccountIM emailAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userName = "admin";//_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            var model = _mapper.Map<EmailAccount>(emailAccount);
            model.Password = EncryptionHelper.Encrypt(model.Password);
            model.CreatedBy = userName;
            model.CreatedDate = DateTime.Now;

            _context.EmailAccounts.Add(model);
            await _context.SaveChangesAsync();

            return Ok(); //CreatedAtAction("GetEmailAccount", new { id = emailAccount.Id }, emailAccount);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> TestAsync(TestEmailIM vm)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var emailAccount = await _context.EmailAccounts.FindAsync(vm.AccountId);
              

                if (!string.IsNullOrEmpty(emailAccount.Password))
                {
                    var pw = EncryptionHelper.Decrypt(emailAccount.Password);
                    emailAccount.Password = pw;
                }


                _emailService.SendMail("TZGOA", vm.TestEmail, vm.TestEmail, string.Empty,
                    "测试", "测试邮件", emailAccount.Smtpserver, emailAccount.Email, string.Empty, emailAccount.UserName,
                    emailAccount.Password, emailAccount.Port, emailAccount.EnableSsl);

            
                return Ok();
            }
            catch (Exception er)
            {
                //AR.Setfailure(er.Message);
                return BadRequest(er.Message);
            }


        }

        // DELETE: api/EmailAccounts/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteEmailAccount([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailAccount = await _context.EmailAccounts.FindAsync(id);
            if (emailAccount == null)
            {
                return NotFound();
            }

            _context.EmailAccounts.Remove(emailAccount);
            await _context.SaveChangesAsync();

            return Ok(emailAccount);
        }

        private bool EmailAccountExists(int id)
        {
            return _context.EmailAccounts.Any(e => e.Id == id);
        }
    }
}