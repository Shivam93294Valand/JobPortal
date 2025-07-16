using System.ComponentModel.DataAnnotations;

namespace JobPortalMVC.Models
{
    public class JobApplicationModel
    {
        public int JobApplicationId { get; set; }

        [Required]
        public int JobId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        //[Display(Name = "Applicant")]
        //public string ApplicantName { get; set; }

        //[Display(Name = "Resume (PDF only)")]
        //[Required(ErrorMessage = "Please upload your resume.")]
        //public IFormFile ResumeFile { get; set; }

        [Required(ErrorMessage = "Please provide your resume URL.")]
        [Display(Name = "Resume")]
        public string Resume { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Applied On")]
        public DateTime AppliedAt { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime UpdatedAt { get; set; }
    }
}