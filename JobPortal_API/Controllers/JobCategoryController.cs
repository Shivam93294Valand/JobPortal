using JobPortalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCategoryController : ControllerBase
    {
        private readonly JobPortalDbContext _context;

        public JobCategoryController(JobPortalDbContext context)
        {
            _context = context;
        }

        #region GetAllJobCategories
        [HttpGet("GetAllJobCategories")]
        public async Task<ActionResult<IEnumerable<JobCategory>>> GetAllJobCategories()
        {
            var jobCategories = await _context.JobCategories.ToListAsync();
            return Ok(jobCategories);
        }
        #endregion

        #region GetJobCategory
        // ADDED: Get by ID is required for the Edit functionality
        [HttpGet("GetJobCategory/{id}")]
        public async Task<ActionResult<JobCategory>> GetJobCategory(int id)
        {
            var jobCategory = await _context.JobCategories.FindAsync(id);
            if (jobCategory == null)
            {
                return NotFound();
            }
            return Ok(jobCategory);
        }
        #endregion

        #region AddJobCategory
        [HttpPost("AddJobCategory")]
        public async Task<ActionResult<JobCategory>> AddJobCategory([FromBody] JobCategory jobCategory)
        {
            // API manages its own timestamps
            jobCategory.CreatedAt = DateTime.UtcNow;
            await _context.JobCategories.AddAsync(jobCategory);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJobCategory), new { id = jobCategory.CategoryId }, jobCategory);
        }
        #endregion

        #region UpdateJobCategory
        [HttpPut("UpdateJobCategory/{id}")]
        public async Task<IActionResult> UpdateJobCategory(int id, [FromBody] JobCategory categoryFromRequest)
        {
            // Step 1: Validate that the ID in the URL matches the ID in the object sent.
            if (id != categoryFromRequest.CategoryId)
            {
                return BadRequest("Mismatched category ID.");
            }

            // Step 2: Fetch the *existing* record from the database. Do not trust the incoming object directly.
            var categoryInDb = await _context.JobCategories.FindAsync(id);

            // Step 3: If the record doesn't exist, return a 404 Not Found.
            if (categoryInDb == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            // Step 4: Copy ONLY the properties that are allowed to be updated.
            // This prevents accidental changes to UserId or CreatedAt.
            categoryInDb.CategoryName = categoryFromRequest.CategoryName;

            // Step 5: Let the API manage its own timestamp for the update.
            categoryInDb.UpdatedAt = DateTime.UtcNow;

            // Step 6: Save the changes. EF knows exactly which fields have been modified.
            await _context.SaveChangesAsync();

            // A 204 No Content response is the standard for a successful PUT request.
            return NoContent();
        }
        #endregion

        #region DeleteJobCategory
        [HttpDelete("DeleteJobCategory/{id}")]
        public async Task<IActionResult> DeleteJobCategory(int id)
        {
            var jobCategory = await _context.JobCategories.FindAsync(id);
            if (jobCategory == null)
            {
                return NotFound();
            }
            _context.JobCategories.Remove(jobCategory);
            await _context.SaveChangesAsync();
            return NoContent(); // Standard for successful DELETE
        }
        #endregion
    }
}