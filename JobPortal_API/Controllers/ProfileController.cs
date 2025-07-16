using JobPortalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        
        #region constructor
        private readonly JobPortalDbContext _context;
        public ProfileController(JobPortalDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Get User Profile

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> Profile(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            _context.Users.Add(user); 
            await _context.SaveChangesAsync();
            return Ok(user);
            //return CreatedAtAction(nameof(Profile), new { userId = user.UserId }, user);
        }

        [HttpPut("Update/{userId}")]
        public async Task<ActionResult> Update(int userId, [FromBody] User user)
        {
            if (userId != user.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            var userInDb = await _context.Users.FindAsync(userId);

            if (userInDb == null)
            {
                return NotFound($"User with Id = {userId} not found");
            }

            userInDb.FirstName = user.FirstName;
            userInDb.LastName = user.LastName;
            userInDb.Email = user.Email;
            userInDb.Role = user.Role;
            userInDb.ImageUrl = user.ImageUrl;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region Login User Profile

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            var userProfile = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

            if (userProfile == null)
            {
                return NotFound("Invalid email or password.");
            }

            return Ok(userProfile);
        }
        #endregion
    }
}