using JobPortalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        #region constructor
        private readonly JobPortalDbContext _context;
        public SkillController(JobPortalDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Get All Skills
        [HttpGet("GetAllSkills")]
        public async Task<ActionResult<IEnumerable<Skill>>> GetAllSkills()
        {
            var skills = await _context.Skills.ToListAsync();
            return Ok(skills);
        }
        #endregion

        #region Get Skill By ID
        [HttpGet("GetSkill/{id}")]
        public async Task<ActionResult<Skill>> GetSkill(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var skill = await _context.Skills.FindAsync(id);
            return Ok(skill);
        }
        #endregion

        #region Add Skill
        [HttpPost("AddSkill")]
        public async Task<ActionResult<Skill>> AddSkill([FromBody] Skill skill)
        {
            try
            {
                if (skill == null)
                {
                    return BadRequest("Skill data is required.");
                }
                
                if (string.IsNullOrEmpty(skill.SkillName))
                {
                    return BadRequest("Skill name is required.");
                }
                
                // Check if skill with same name already exists
                var existingSkill = await _context.Skills.FirstOrDefaultAsync(s => s.SkillName == skill.SkillName);
                if (existingSkill != null)
                {
                    return Conflict($"Skill with name '{skill.SkillName}' already exists.");
                }
                
                var now = DateTime.UtcNow;
                skill.CreatedAt = now;
                skill.UpdatedAt = now;
                
                await _context.Skills.AddAsync(skill);
                await _context.SaveChangesAsync();
                
                return CreatedAtAction(nameof(GetSkill), new { id = skill.SkillId }, skill);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding skill: {ex.Message}");
            }
        }
        #endregion

        #region Update Skill
        [HttpPut("UpdateSkill/{id}")]
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] Skill skillFromRequest)
        {
            if (id != skillFromRequest.SkillId)
            {
                return BadRequest("Mismatched skill ID.");
            }

            var skillInDb = await _context.Skills.FindAsync(id);
            if (skillInDb == null)
            {
                return NotFound(($"Skill with ID {id} not found."));
            }

            skillInDb.SkillName = skillFromRequest.SkillName;
            skillInDb.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                if (_context.Skills.Any (e => e.SkillId == id))
                {
                    NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        #endregion

        #region Delete Skill
        [HttpDelete("DeleteSkill/{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var skill = await _context.Skills.FindAsync(id);
            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
            return Ok(skill);
        }
        #endregion

        #region select top 10 skills
        [HttpGet("GetTopTenskills")]
        public async Task<ActionResult<Skill>> GetTopTenSkills()
        {
            var Skills = await _context.Skills.Take(10).ToListAsync();
            return Ok(Skills);
        }
        #endregion

        #region find skill by name
        [HttpGet("GetSkillByName/{name}")]
        public async Task<ActionResult<Skill>> GetSkillByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Skill name cannot be null or empty.");
            }
            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.SkillName == name);
            if (skill == null)
            {
                return NotFound($"Skill with name '{name}' not found.");
            }
            return Ok(skill);
        }
        #endregion
    }
}