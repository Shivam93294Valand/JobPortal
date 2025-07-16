using JobPortalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavedJobController : ControllerBase
    {
        #region constructor
        private readonly JobPortalDbContext _context;
        public SavedJobController(JobPortalDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Get All Saved Jobs
        [HttpGet("GetAllSavedJobs")]
        public async Task<ActionResult<ActionResult<SavedJob>>> GetSavedJobs()
        {
            var savedJobs = await _context.SavedJobs.ToListAsync();
            return Ok(savedJobs);
        }
        #endregion

        #region Get Saved Job By Id
        [HttpGet("GetSavedJob/{id}")]
        public async Task<ActionResult<SavedJob>> GetSavedJob(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var savedJob = await _context.SavedJobs.FindAsync(id);
            return Ok(savedJob);
        }
        #endregion

        #region Add Saved Job
        [HttpPost("AddSavedJob")]
        public async Task<ActionResult<SavedJob>> AddSavedJob([FromBody] SavedJob savedJob)
        {
            await _context.SavedJobs.AddAsync(savedJob);
            _context.SaveChanges();
            return Ok(savedJob);
        }
        #endregion

        #region Update Saved Job
        [HttpPut("UpdateSavedJob/{id}")] 
        public async Task<ActionResult<SavedJob>> UpdateSavedJob(int id, [FromBody] SavedJob savedJob)
        {
            if (id == null)
            {
                return NotFound();
            }
            _context.SavedJobs.Update(savedJob);
            await _context.SaveChangesAsync();
            return Ok(savedJob);
        }
        #endregion

        #region Delete Saved Job By Id
        [HttpDelete("DeleteSavedJob/{id}")] 
        public async Task<ActionResult<SavedJob>> DeleteSavedJob(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var savedJob = await _context.SavedJobs.FindAsync(id);
            _context.SavedJobs.Remove(savedJob);
            await _context.SaveChangesAsync();
            return Ok(savedJob);
        }
        #endregion
    }
}
