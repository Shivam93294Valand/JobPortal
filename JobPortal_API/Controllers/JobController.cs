using JobPortalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        #region Constructor
        private readonly JobPortalDbContext _context;
        public JobController(JobPortalDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllJobs
        [HttpGet("GetAllJobs")]
        public async Task<ActionResult<Job>> GetAllJobs()
        {
            var jobs = await _context.Jobs.ToListAsync();
            return Ok(jobs);
        }
        #endregion

        #region AddJob
        [HttpPost("AddJob")]
        public async Task<ActionResult<Job>> AddJob([FromBody] Job job)
        {
            await _context.Jobs.AddAsync(job);
            _context.SaveChanges();
            return Ok(job);
        }
        #endregion

        #region Update Job By Id
        [HttpPut("UpdateJob/{id}")] 
        public async Task<ActionResult<Job>> UpdateJob(int id, [FromBody] Job job)
        {
            if (id == null)
            {
                return NotFound();
            }
            job.UpdatedAt = DateTime.Now;
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
            return Ok(job);
        }
        #endregion

        #region Delete Job By Id
        [HttpDelete("DeleteJob/{id}")] 
        public async Task<ActionResult<Job>> DeleteJob(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var job = await _context.Jobs.FindAsync(id);
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return Ok(job);
        }
        #endregion
    }
}
