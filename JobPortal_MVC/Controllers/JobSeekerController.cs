using JobPortalMVC.Models;         
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Diagnostics;           

namespace JobPortalMVC.Controllers
{
    public class JobSeekerController : Controller
    {
        #region Dashboard
        [Route("/JobSeeker")]
        public IActionResult Dashboard()
        {
            return View();
        }
        #endregion

        #region Skills
        public IActionResult AddSkills()
        { 
            return View();
        }

        public IActionResult SkillsList()
        {
            var allSkills = new List<string>
            {
                "ASP.NET Core",
                "Azure",
                "Blazor",
                "C#",
                "CSS",
                "DevOps",
                "Entity Framework Core",
                "Git",
                "HTML",
                "JavaScript",
                "jQuery",
                "Microsoft SQL Server",
                "React",
                "RESTful APIs",
                "TypeScript",
                "Vue.js"
            };

            var model = new SkillsListModel
            {
                Skills = allSkills
            };

            return View(model);
        }

        //[HttpPost]
        //public IActionResult AddSkills(SkillsListModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Debug.WriteLine($"User submitted {model.Skills.Count} skills.");
        //        foreach (var skill in model.Skills)
        //        {
        //            Debug.WriteLine($"- Skill: {skill}");
        //        }
        //        return RedirectToAction("Dashboard");
        //    }
        //    return View(model);
        //}

        //public IActionResult SkillsList()
        //{
        //    return View();
        //}
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