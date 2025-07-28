using JobPortalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace JobPortalMVC.Controllers
{
    public class EmployeerController : Controller
    {
        #region Constructor
        private readonly HttpClient _client;
        public EmployeerController(IHttpClientFactory httpclientFactory)
        {
            _client = httpclientFactory.CreateClient("JobPortalAPI");
            _client.BaseAddress = new Uri("https://localhost:7172/api/");
        }
        #endregion

        #region Dashboard
        [Route("/Employeer")]
        public IActionResult Dashboard()
        {
            return View();
        }
        #endregion

        #region Companies
        public async Task<ActionResult> CompaniesList()
        {
            var response = await _client.GetAsync("Company");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<CompanyModel>>(json);
            return View(list);
        }

        public async Task<ActionResult> DeleteCompany(int id)
        {
            var response = await _client.DeleteAsync($"Company/DeleteCompany/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("CompaniesList");
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> AddEditCompany(int? id)
        {
            if (id == null || id == 0)
            {
                ViewData["Title"] = "Create Company Profile";
                return View(new CompanyModel());
            }
            else 
            {
                ViewData["Title"] = "Edit Company Profile";

                var response = await _client.GetAsync($"Company/GetCompany/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var company = JsonConvert.DeserializeObject<CompanyModel>(json);
                    if (company != null)
                    {
                        return View(company);
                    }
                }
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditCompany(CompanyModel company)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = company.CompanyId == 0 ? "Create Company Profile" : "Edit Company Profile";
                return View(company);
            }

            if (company.CompanyId == 0)
            {
                company.UserId = 1;

                var json = JsonConvert.SerializeObject(company);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("Company/AddCompany", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("CompaniesList");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API Error: Could not create the company.");
                }
            }
            else
            {
                var json = JsonConvert.SerializeObject(company);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"Company/UpdateCompany/{company.CompanyId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("CompaniesList");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API Error: Could not update the company.");
                }
            }

            ViewData["Title"] = company.CompanyId == 0 ? "Create Company Profile" : "Edit Company Profile";
            return View(company);
        }
        #endregion

        #region Catagory

        public async Task<ActionResult> CategoryList()
        {
            var response = await _client.GetAsync("JobCategory/GetAllJobCategories");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<JobCatagoryModel>>(json);
            return View(list);
        }

        public async Task<ActionResult> DeleteCategory(int id)
        {
            var response = await _client.DeleteAsync($"JobCategory/DeleteJobCategory/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("CategoryList");
            }
            else
            {
                TempData["ErrorMessage"] = "Could not delete category. It might be in use.";
                return RedirectToAction("CategoryList");
            }
        }

        public async Task<IActionResult> AddEditCategoryAsync(int? id)
        {
            if (id == null || id == 0)
            {
                ViewData["Title"] = "Create Job Category";
                return View(new JobCatagoryModel());
            }
            else
            {
                ViewData["Title"] = "Edit Job Category";
                var response = await _client.GetAsync($"JobCategory/GetJobCategory/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var category = JsonConvert.DeserializeObject<JobCatagoryModel>(json);
                    return View(category);
                }
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditCategory(JobCatagoryModel category) 
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = category.CategoryId == 0 ? "Create Job Category" : "Edit Job Category";
                return View(category);
            }

            if (category.CategoryId == 0) 
            {
                category.UserId = 1; 
                var json = JsonConvert.SerializeObject(category);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("JobCategory/AddJobCategory", content);
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "API Error: Could not create category.");
                }
            }
            else 
            {
                var json = JsonConvert.SerializeObject(category);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"JobCategory/UpdateJobCategory/{category.CategoryId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("CategoryList");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API Error ({response.StatusCode}): {errorContent}");

                    ViewData["Title"] = "Edit Job Category";
                    return View(category);
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                ViewData["Title"] = category.CategoryId == 0 ? "Create Job Category" : "Edit Job Category";
                return View(category);
            }

            return RedirectToAction("CategoryList");
        }
        #endregion

        #region Jobs
        public async Task <IActionResult> JobsList()
        {
            var response = await _client.GetAsync("Job/GetAllJobs");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<JobModel>>(json);
            return View(list);
        }

        public async Task<ActionResult> DeleteJob(int id)
        {
            var response = await _client.DeleteAsync($"Job/DeleteJob/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("JobsList");
            }
            else
            {
                TempData["ErrorMessage"] = "Could not delete Job. It might be in use.";
                return RedirectToAction("JobsList");
            }
        }

        public async Task<IActionResult> AddEditJob(int? id)
        {
            var jobModel = new JobModel();

            if (id == null || id == 0) 
            {
                ViewData["Title"] = "Create Job";
            }
            else 
            {
                ViewData["Title"] = "Edit Job";
                var response = await _client.GetAsync($"Job/GetJob/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    jobModel = JsonConvert.DeserializeObject<JobModel>(json);
                    if (jobModel == null) return NotFound();
                }
                else
                {
                    return NotFound();
                }
            }
            await PopulateJobFormDropdowns(jobModel);
            return View(jobModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditJob(JobModel job)
        {
            await PopulateJobFormDropdowns(job);

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = job.JobId == 0 ? "Create Job" : "Edit Job";
                return View(job);
            }

            if (job.JobId == 0) 
            {
                job.UserId = 1;

                var json = JsonConvert.SerializeObject(job);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("Job/AddJob", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"API Error: Could not create job. {errorContent}");
                    ViewData["Title"] = "Create Job";
                    return View(job); 
                }
            }
            else 
            {
                var json = JsonConvert.SerializeObject(job);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"Job/UpdateJob/{job.JobId}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API Error ({response.StatusCode}): {errorContent}");

                    ViewData["Title"] = "Edit Job";
                    return View(job); 
                }
            }
            return RedirectToAction("JobsList");
        }

        private async Task PopulateJobFormDropdowns(JobModel jobModel)
        {
            // Fetch Companies 
            var companyResponse = await _client.GetAsync("Company");
            if (companyResponse.IsSuccessStatusCode)
            {
                var companyJson = await companyResponse.Content.ReadAsStringAsync();
                var companies = JsonConvert.DeserializeObject<List<CompanyModel>>(companyJson);
                jobModel.Companies = companies.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CompanyId.ToString()
                }).ToList();
            }

            // Fetch Categories 
            var categoryResponse = await _client.GetAsync("JobCategory/GetAllJobCategories");
            if (categoryResponse.IsSuccessStatusCode)
            {
                var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<JobCatagoryModel>>(categoryJson);
                jobModel.Categories = categories.Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.CategoryId.ToString()
                }).ToList();
            }

            var skillResponse = await _client.GetAsync("Skill/GetAllSkills");
            if (skillResponse.IsSuccessStatusCode)
            {
                var skillJson = await skillResponse.Content.ReadAsStringAsync();
                var skills = JsonConvert.DeserializeObject<List<SkillModel>>(skillJson);
                jobModel.SkillsList = skills.Select(s => new SelectListItem
                {
                    Text = s.SkillName,
                    Value = s.SkillId.ToString()
                }).ToList();
            }
        }
        #endregion

        #region Applicants
        public IActionResult ApplicantsList()
        {
            return View();
        }
        #endregion

        #region Skills
        public async Task<ActionResult> SkillsList()
        {
            var response = await _client.GetAsync("Skill/GetAllSkills");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<SkillModel>>(json);
            return View(list);
        }

        public async Task<ActionResult> DeleteSkill(int id)
        {
            var response = await _client.DeleteAsync($"Skill/DeleteSkill/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("SkillsList");
            }
            else
            {
                TempData["ErrorMessage"] = "Could not delete Skill. It might be in use.";
                return RedirectToAction("SkillsList");
            }
        }

        public async Task<IActionResult> AddEditSkillAsync(int? id)
        {
            if (id == null || id == 0)
            {
                ViewData["Title"] = "Create Skill";
                return View(new SkillModel());
            }
            else
            {
                ViewData["Title"] = "Edit Skill";
                var response = await _client.GetAsync($"Skill/GetSkill/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var skill = JsonConvert.DeserializeObject<SkillModel>(json);
                    return View(skill);
                }
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditSkill(SkillModel skill) 
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = skill.SkillId == 0 ? "Create Skill" : "Edit Skill";
                return View(skill);
            }

            if (skill.SkillId == 0) 
            {
                skill.UserId = 1; 
                var json = JsonConvert.SerializeObject(skill);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("Skill/AddSkill", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("SkillsList");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API Error ({response.StatusCode}): {errorContent}");
                }
            }
            else 
            {
                var json = JsonConvert.SerializeObject(skill);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"Skill/UpdateSkill/{skill.SkillId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("SkillsList");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API Error ({response.StatusCode}): {errorContent}");
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                ViewData["Title"] = skill.SkillId == 0 ? "Create Skill" : "Edit Skill";
                return View(skill);
            }

            return RedirectToAction("SkillsList");
        }
        #endregion
    }
}
