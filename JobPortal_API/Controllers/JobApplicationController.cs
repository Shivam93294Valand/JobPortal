using JobPortalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        #region constructor
        private readonly JobPortalDbContext _context;
        public JobApplicationController(JobPortalDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllJobApplications
        [HttpGet("GetAllJobApplications")]
        public async Task<ActionResult<JobApplication>> GetAllJobApplications()
        {
            var jobApplications = await _context.JobApplications.ToListAsync();
            return Ok(jobApplications);
        }
        #endregion

        #region GetJobApplication By Id
        [HttpGet("GetJobApplication/{id}")] 
        public async Task<ActionResult<JobApplication>> GetJobApplication(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var jobApplication = await _context.JobApplications.FindAsync(id);
            return Ok(jobApplication);
        }
        #endregion

        #region AddJobApplication
        [HttpPost("AddJobApplication")]
        public async Task<ActionResult<JobApplication>> AddJobApplication(JobApplication jobApplication)
        {
            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();
            return Ok(jobApplication);
        }
        #endregion

        #region UpdateJobApplication By Id
        [HttpPut("UpdateJobApplication/{id}")] 
        public async Task<ActionResult<JobApplication>> UpdateJobApplication(int id, [FromBody] JobApplication jobApplication)
        {
            if (id == null)
            {
                return NotFound();
            }
            _context.JobApplications.Update(jobApplication);
            await _context.SaveChangesAsync();
            return Ok(jobApplication);
        }
        #endregion

        #region DeleteJobApplication By Id
        [HttpDelete("DeleteJobApplication/{id}")] 
        public async Task<ActionResult<JobApplication>> DeleteJobApplication(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var jobApplication = await _context.JobApplications.FindAsync(id);
            _context.JobApplications.Remove(jobApplication);
            await _context.SaveChangesAsync();
            return Ok(jobApplication);
        }
        #endregion

        #region SearchJobApplication By Job Title
        [HttpGet("SearchJobApplicationByJobTitle/{jobTitle}")]
        public async Task<ActionResult<JobApplication>> SearchJobApplicationByJobTitle(string jobTitle)
        {
            var jobApplication = await _context.JobApplications.Include(x => x.Job)
                .Where(x => x.Job.Title == jobTitle).ToListAsync();
            return Ok(jobApplication);
        }
        #endregion

        #region SearchJobApplication By Location
        [HttpGet("SearchJobApplicationByLocation/{location}")]
        public async Task<ActionResult<JobApplication>> SearchJobApplicationByLocation(string location)
        {
            var jobApplication = await _context.JobApplications.Include(x => x.Job)
                .Where(x => x.Job.Location == location).ToListAsync();
            return Ok(jobApplication);
        }
        #endregion
    }
}
