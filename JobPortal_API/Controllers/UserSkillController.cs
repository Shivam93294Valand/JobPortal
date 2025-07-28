using JobPortalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSkillController : ControllerBase
    {
        #region constructor
        private readonly JobPortalDbContext _context;
        public UserSkillController(JobPortalDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAll UserSkills
        [HttpGet("GetAllUserSkills")]
        public async Task<ActionResult<List<UserSkill>>> GetAllUserSkills()
        {
            var userSkills = await _context.UserSkills.ToListAsync();
            return Ok(userSkills);
        }
        #endregion

        #region Add UserSkill
        [HttpPost("AddUserSkill")]
        public async Task<ActionResult<UserSkill>> AddUserSkill([FromBody] UserSkill userSkill)
        {
            try
            {
                // Check if the user-skill combination already exists
                var existingUserSkill = await _context.UserSkills
                    .FirstOrDefaultAsync(us => us.UserId == userSkill.UserId && us.SkillId == userSkill.SkillId);

                if (existingUserSkill != null)
                {
                    return Conflict("User already has this skill.");
                }

                userSkill.CreatedAt = DateTime.UtcNow;
                await _context.UserSkills.AddAsync(userSkill);
                await _context.SaveChangesAsync();
                return Ok(userSkill);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding user skill: {ex.Message}");
            }
        }
        #endregion

        #region Update UserSkill By ID
        [HttpPut("UpdateUserSkill/{id}")]
        public async Task<ActionResult<UserSkill>> UpdateUserSkill(int id, [FromBody] UserSkill userSkill)
        {
            if (id == null)
            {
                return NotFound();
            }
            _context.UserSkills.Update(userSkill);
            await _context.SaveChangesAsync();
            return Ok(userSkill);
        }
        #endregion

        //#region Delete UserSkill By ID
        //[HttpDelete("DeleteUserSkill/{id}")]
        //public async Task<ActionResult<UserSkill>> DeleteUserSkill(int id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var userSkill = await _context.UserSkills.FindAsync(id);
        //    _context.UserSkills.Remove(userSkill);
        //    await _context.SaveChangesAsync();
        //    return Ok(userSkill);
        //}
        //#endregion

        #region Delete UserSkill by UserId and SkillId
        [HttpDelete("DeleteUserSkill/{userId}/{skillId}")]
        public async Task<IActionResult> DeleteUserSkill(int userId, int skillId)
        {
            var userSkill = await _context.UserSkills
                .FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId);

            if (userSkill == null)
            {
                return NotFound("The specified user skill was not found.");
            }

            _context.UserSkills.Remove(userSkill);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}