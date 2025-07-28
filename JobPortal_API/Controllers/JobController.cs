using JobPortalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly JobPortalDbContext _context;
        public JobController(JobPortalDbContext context)
        {
            _context = context;
        }

        #region Get All Jobs
        [HttpGet("GetAllJobs")]
        public async Task<ActionResult<IEnumerable<Job>>> GetAllJobs()
        {
            var jobs = await _context.Jobs.Include(j => j.Company).ToListAsync();
            return Ok(jobs);
        }
        #endregion

        #region Get Job By Id
        [HttpGet("GetJob/{id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }
        #endregion

        #region AddJob 
        [HttpPost("AddJob")]
        public async Task<ActionResult<Job>> AddJob([FromBody] Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var now = DateTime.UtcNow;
            job.CreatedAt = now;
            job.UpdatedAt = now;

            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync(); 

            return CreatedAtAction(nameof(GetJob), new { id = job.JobId }, job);
        }
        #endregion

        #region Update Job By Id 
        [HttpPut("UpdateJob/{id}")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] Job jobFromRequest)
        {
            if (id != jobFromRequest.JobId)
            {
                return BadRequest("Mismatched Job ID.");
            }

            var jobInDb = await _context.Jobs.FindAsync(id);
            if (jobInDb == null)
            {
                return NotFound($"Job with ID {id} not found.");
            }

            // Update all the properties from the incoming request
            jobInDb.Title = jobFromRequest.Title;
            jobInDb.Description = jobFromRequest.Description;
            jobInDb.KeyResponsibilities = jobFromRequest.KeyResponsibilities;
            jobInDb.Qualifications = jobFromRequest.Qualifications;
            jobInDb.IsActive = jobFromRequest.IsActive;
            jobInDb.Location = jobFromRequest.Location;
            jobInDb.SalaryRange = jobFromRequest.SalaryRange;
            jobInDb.ExperienceRange = jobFromRequest.ExperienceRange;
            jobInDb.JobType = jobFromRequest.JobType;
            jobInDb.ExpiresAt = jobFromRequest.ExpiresAt;
            jobInDb.CategoryId = jobFromRequest.CategoryId;
            jobInDb.CompanyId = jobFromRequest.CompanyId;

            jobInDb.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Jobs.Any(e => e.JobId == id))
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
        #endregion

        #region Delete Job By Id
        [HttpDelete("DeleteJob/{id}")]
        public async Task<ActionResult<Job>> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return Ok(job);
        }
        #endregion
    }
}