using JobPortalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyReviewController : ControllerBase
    {
        #region constructor
        private readonly JobPortalDbContext _context;
        public CompanyReviewController(JobPortalDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllReviews
        [HttpGet("GetAllReviews")]
        public async Task<ActionResult<CompanyReview>> GetAllReviews()
        {
            var reviews = await _context.CompanyReviews.ToListAsync();
            return Ok(reviews);
        }
        #endregion

        #region AddReview
        [HttpPost("AddReview")]
        public async Task<ActionResult<CompanyReview>> AddReview([FromBody] CompanyReview review)
        {
            await _context.CompanyReviews.AddAsync(review);
            _context.SaveChanges();
            return Ok(review);
        }
        #endregion

        #region UpdateReview By Id
        [HttpPut("UpdateReview/{id}")] 
        public async Task<ActionResult<CompanyReview>> UpdateReview(int id, [FromBody] CompanyReview review)
        {
            if (id == null)
            {
                return NotFound();
            }
            _context.CompanyReviews.Update(review);
            await _context.SaveChangesAsync();
            return Ok(review);
        }
        #endregion

        #region DeleteReview By Id
        [HttpDelete("DeleteReview/{id}")] 
        public async Task<ActionResult<CompanyReview>> DeleteReview(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var review = await _context.CompanyReviews.FindAsync(id);
            _context.CompanyReviews.Remove(review);
            await _context.SaveChangesAsync();
            return Ok(review);
        }
        #endregion

        #region top 5 reviews
        [HttpGet("GetTop5Reviews")]
        public async Task<ActionResult<CompanyReview>> GetTop5Reviews()
        {
            var reviews = await _context.CompanyReviews.ToListAsync();
            return Ok(reviews);
        }
        #endregion
    }
}
