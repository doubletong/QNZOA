using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGOA.Data;

namespace SIGOA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTemplatesController : ControllerBase
    {
        private readonly SigbugsdbContext _context;
        private readonly IMapper _mapper;
        public EmailTemplatesController(SigbugsdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/EmailTemplates
        [HttpGet]
        public IEnumerable<EmailTemplate> GetEmailTemplates()
        {
            return _context.EmailTemplates;
        }



        // GET: api/EmailTemplates/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmailTemplate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailTemplate = await _context.EmailTemplates.FindAsync(id);

            if (emailTemplate == null)
            {
                return NotFound();
            }

            return Ok(emailTemplate);
        }

        // PUT: api/EmailTemplates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmailTemplate([FromRoute] int id, [FromBody] EmailTemplate emailTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emailTemplate.Id)
            {
                return BadRequest();
            }

            _context.Entry(emailTemplate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailTemplateExists(id))
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

        // POST: api/EmailTemplates
        [HttpPost]
        public async Task<IActionResult> PostEmailTemplate([FromBody] EmailTemplate emailTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EmailTemplates.Add(emailTemplate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmailTemplate", new { id = emailTemplate.Id }, emailTemplate);
        }

        // DELETE: api/EmailTemplates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmailTemplate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailTemplate = await _context.EmailTemplates.FindAsync(id);
            if (emailTemplate == null)
            {
                return NotFound();
            }

            _context.EmailTemplates.Remove(emailTemplate);
            await _context.SaveChangesAsync();

            return Ok(emailTemplate);
        }

        private bool EmailTemplateExists(int id)
        {
            return _context.EmailTemplates.Any(e => e.Id == id);
        }
    }
}