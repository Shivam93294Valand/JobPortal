using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JobPortalMVC.Models
{
    public class JobModel
    {
        public int JobId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Key Responsibilities is required")]
        public string KeyResponsibilities { get; set; } = null!;

        [Required(ErrorMessage = "Qualifications are required")]
        public string Qualifications { get; set; } = null!;

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = null!;

        public string? SalaryRange { get; set; } 

        public string? ExperienceRange { get; set; } 

        [Required(ErrorMessage = "Job Type is required")]
        public string JobType { get; set; } = null!;

        [Required(ErrorMessage = "Date is required")]
        public DateOnly ExpiresAt { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Company is required")]
        public int CompanyId { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Skill is required")]
        public int SkillId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public IEnumerable<SelectListItem> Companies { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> SkillsList { get; set; } = new List<SelectListItem>();
    }
}