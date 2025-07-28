using JobPortalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace JobPortalMVC.Controllers
{
    public class JobSeekerController : Controller
    {
        #region Constructor
        private readonly HttpClient _client;
        private readonly ILogger<JobSeekerController> _logger;

        public JobSeekerController(IHttpClientFactory httpclientFactory, ILogger<JobSeekerController> logger)
        {
            _client = httpclientFactory.CreateClient("JobPortalAPI");
            _client.BaseAddress = new Uri("https://localhost:7172/api/");
            _logger = logger;
        }
        #endregion

        #region Dashboard
        [Route("/JobSeeker")]
        public IActionResult Dashboard()
        {
            return View();
        }
        #endregion

        #region Skills
        public IActionResult AddEditSkills()
        {
            return View(new UserSkillModel());
        }

        public async Task<IActionResult> SkillsList()
        {
            const int currentUserId = 1;
            var viewModel = new SkillsListViewModel();

            try
            {
                var skillsResponse = await _client.GetAsync("Skill/GetAllSkills");
                if (!skillsResponse.IsSuccessStatusCode)
                {
                    string errorContent = await skillsResponse.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = "Could not retrieve master skills list from API.";
                    return View(viewModel);
                }

                var skillsJson = await skillsResponse.Content.ReadAsStringAsync();
                var allSkills = JsonSerializer.Deserialize<List<SkillModel>>(skillsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var skillIdToNameMap = allSkills.ToDictionary(s => s.SkillId, s => s.SkillName);

                var userSkillsResponse = await _client.GetAsync("UserSkill/GetAllUserSkills");
                if (!userSkillsResponse.IsSuccessStatusCode)
                {
                    string errorContent = await userSkillsResponse.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = "Could not retrieve user skills from API.";
                    return View(viewModel);
                }

                var userSkillsJson = await userSkillsResponse.Content.ReadAsStringAsync();
                var allUserSkills = JsonSerializer.Deserialize<List<UserSkillModel>>(userSkillsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                viewModel.Skills = allUserSkills
                    .Where(us => us.UserId == currentUserId)
                    .Select(us => new UserSkillModel
                    {
                        SkillId = us.SkillId,
                        SkillName = skillIdToNameMap.GetValueOrDefault(us.SkillId)
                    })
                    .Where(s => s.SkillName != null)
                    .OrderBy(s => s.SkillName)
                    .ToList();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            }

            return View(viewModel);
        }
        #endregion

        #region User Skill
        [HttpPost]
        public async Task<IActionResult> AddUserSkill([FromBody] UserSkillModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.SkillName))
                {
                    return BadRequest("Skill name is required.");
                }

                int currentUserId = 1;
                var skillName = model.SkillName.Trim();
                SkillModel skillFromApi = null;

                var skillResponse = await _client.GetAsync($"Skill/GetSkillByName/{Uri.EscapeDataString(skillName)}");

                if (skillResponse.IsSuccessStatusCode)
                {
                    var skillJson = await skillResponse.Content.ReadAsStringAsync();
                    skillFromApi = JsonSerializer.Deserialize<SkillModel>(skillJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else if (skillResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var newSkill = new SkillModel { SkillName = skillName, UserId = currentUserId };
                    var newSkillJson = JsonSerializer.Serialize(newSkill);
                    var content = new StringContent(newSkillJson, System.Text.Encoding.UTF8, "application/json");

                    var createSkillResponse = await _client.PostAsync("Skill/AddSkill", content);
                    if (createSkillResponse.IsSuccessStatusCode)
                    {
                        var createdSkillJson = await createSkillResponse.Content.ReadAsStringAsync();
                        skillFromApi = JsonSerializer.Deserialize<SkillModel>(createdSkillJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    else
                    {
                        return StatusCode((int)createSkillResponse.StatusCode, "Failed to create skill.");
                    }
                }
                else
                {
                    return StatusCode((int)skillResponse.StatusCode, "Error while checking for skill.");
                }

                var userSkill = new UserSkillModel { UserId = currentUserId, SkillId = skillFromApi.SkillId };
                var userSkillJson = JsonSerializer.Serialize(userSkill);
                var userSkillContent = new StringContent(userSkillJson, System.Text.Encoding.UTF8, "application/json");

                var addUserSkillResponse = await _client.PostAsync("UserSkill/AddUserSkill", userSkillContent);

                if (addUserSkillResponse.IsSuccessStatusCode || addUserSkillResponse.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return Json(new { success = true, skill = skillFromApi });
                }
                else
                {
                    return StatusCode((int)addUserSkillResponse.StatusCode, "Failed to add skill to user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user skill.");
                return StatusCode(500, $"An internal server error occurred: {ex.Message}");
            }
        }
        #endregion

        #region Delete User Skill
        [HttpDelete]
        public async Task<IActionResult> DeleteUserSkill(int skillId)
        {
            try
            {
                const int currentUserId = 1;

                var response = await _client.DeleteAsync($"UserSkill/DeleteUserSkill/{currentUserId}/{skillId}");

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound("The skill to delete was not found.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to delete user skill. Status: {StatusCode}, Content: {ErrorContent}", response.StatusCode, errorContent);
                    return StatusCode((int)response.StatusCode, "Failed to delete skill.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user skill.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
        #endregion

        #region Update User Skill
        [HttpGet]
        public async Task<IActionResult> EditSkill(int id)
        {
            try
            {
                const int currentUserId = 1;
                
                // Get the skill details
                var skillsResponse = await _client.GetAsync("Skill/GetAllSkills");
                if (!skillsResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("SkillsList");
                }

                var skillsJson = await skillsResponse.Content.ReadAsStringAsync();
                var allSkills = JsonSerializer.Deserialize<List<SkillModel>>(skillsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                // Find the skill with the matching ID
                var skill = allSkills.FirstOrDefault(s => s.SkillId == id);
                if (skill == null)
                {
                    return RedirectToAction("SkillsList");
                }

                var userSkill = new UserSkillModel
                {
                    SkillId = skill.SkillId,
                    SkillName = skill.SkillName,
                    UserId = currentUserId
                };

                return View("AddEditSkills", userSkill);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the skill for editing.");
                return RedirectToAction("SkillsList");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserSkill([FromBody] UserSkillModel model)
        {
            try
            {
                if (model == null || model.SkillId <= 0 || string.IsNullOrWhiteSpace(model.SkillName))
                {
                    return BadRequest("Invalid skill data.");
                }

                const int currentUserId = 1;
                
                // Update the skill name in the Skill table
                var updateSkill = new SkillModel
                {
                    SkillId = model.SkillId,
                    SkillName = model.SkillName.Trim(),
                    UserId = currentUserId
                };
                
                var updateSkillJson = JsonSerializer.Serialize(updateSkill);
                var content = new StringContent(updateSkillJson, System.Text.Encoding.UTF8, "application/json");
                
                var updateResponse = await _client.PutAsync($"Skill/UpdateSkill/{model.SkillId}", content);
                
                if (updateResponse.IsSuccessStatusCode)
                {
                    return Json(new { success = true, skill = updateSkill });
                }
                else
                {
                    var errorContent = await updateResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to update skill. Status: {StatusCode}, Content: {ErrorContent}", updateResponse.StatusCode, errorContent);
                    return StatusCode((int)updateResponse.StatusCode, "Failed to update skill.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the skill.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
        #endregion

        #region Job Applications
        public IActionResult JobApplicationsList()
        {
            return View();
        }

        public IActionResult JobApplication()
        {
            return View();
        }

        public IActionResult AppliedJobList()
        {
            return View();
        }

        public IActionResult SavedJobsList()
        {
            return View();
        }

        public IActionResult JobDetails()
        {
            return View();
        }
        #endregion

        #region CompanyReviews
        public IActionResult CompanyReviewsList()
        {
            return View();
        }

        public IActionResult AddCompanyReview()
        {
            return View();
        }
        #endregion
    }
}