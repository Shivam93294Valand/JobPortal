using JobPortalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        #region Constructor
        private readonly JobPortalDbContext _context;
        public CompanyController(JobPortalDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllCompanies
        [HttpGet] 
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            var companies = await _context.Companies.ToListAsync();
            return Ok(companies);
        }
        #endregion

        #region GetCompany ById
        [HttpGet("GetCompany/{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var company = await _context.Companies.FindAsync(id);
            return Ok(company);
        }
        #endregion

        #region AddCompany
        [HttpPost("AddCompany")]
        public async Task<ActionResult<Company>> AddCompany([FromBody] Company company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
            return Ok(company);
        }
        #endregion

        #region UpdateCompany ById
        [HttpPut("UpdateCompany/{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] Company companyFromRequest)
        {
            // Step 1: Validate the request. Ensure the ID in the URL matches the one in the body.
            if (id != companyFromRequest.CompanyId)
            {
                return BadRequest("Mismatched company ID in URL and request body.");
            }

            // Step 2: Fetch the EXISTING company record from the database.
            var companyInDb = await _context.Companies.FindAsync(id);

            // Step 3: Check if the company actually exists.
            if (companyInDb == null)
            {
                return NotFound($"Company with ID {id} not found.");
            }

            // Step 4: Manually copy the updatable properties from the request to the entity we just fetched.
            // This prevents overwriting critical data like CreatedAt or UserId.
            companyInDb.Name = companyFromRequest.Name;
            companyInDb.Website = companyFromRequest.Website;
            companyInDb.Description = companyFromRequest.Description;
            companyInDb.LogoUrl = companyFromRequest.LogoUrl;

            // Step 5: Set the 'UpdatedAt' timestamp. The database now knows this record was changed.
            companyInDb.UpdatedAt = DateTime.UtcNow;

            // Step 6: Save the changes. EF is tracking `companyInDb` and knows exactly which fields changed.
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // This is a good practice for handling rare race conditions.
                if (!_context.Companies.Any(e => e.CompanyId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // A 204 No Content is the standard, successful response for a PUT request.
            return NoContent();
        }
        #endregion

        #region DeleteCompany ById
        [HttpDelete("DeleteCompany/{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return Ok(company);
        }
        #endregion

        #region select top 10 companies
        [HttpGet("GetTopTenCompanies")]
        public async Task<ActionResult<Company>> GetTopTenCompanies()
        {
            var companies = await _context.Companies.Take(10).ToListAsync();
            return Ok(companies);
        }
        #endregion

        #region find company by name
        [HttpGet("GetCompanyByName/{name}")] 
        public async Task<ActionResult<Company>> GetCompanyByName(string name)
        {
            if(name == null)
            { 
                return NotFound();
            }
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == name);
            return Ok(company);
        }
        #endregion
    }
}