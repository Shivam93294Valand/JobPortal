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

        [Required(ErrorMessage = "Skills are required")]
        public string Skills { get; set; } = null!;

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "Salary Range is required")]
        public string? SalaryRange { get; set; }

        [Required(ErrorMessage = "Experience Range is required")]
        public string? ExperienceRange { get; set; }

        [Required(ErrorMessage = "Job Type is required")]
        public string JobType { get; set; } = null!;

        [Required(ErrorMessage = "Date is required")]
        public DateOnly ExpiresAt { get; set; }

        public int CategoryId { get; set; }

        public int CompanyId { get; set; }

        public int UserId { get; set; }

        public int SkillId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
