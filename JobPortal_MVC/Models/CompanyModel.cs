using System.ComponentModel.DataAnnotations;

namespace JobPortalMVC.Models
{
    public class CompanyModel
    {
        [Key]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(100)]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Website URL cannot be longer than 200 characters.")]
        [DataType(DataType.Url)]
        [Display(Name = "Company Website")]
        public string? Website { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Company Description")]
        public string? Description { get; set; } 

        [Display(Name = "Company Logo")]
        public string? LogoUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public int UserId { get; set; }

        //public IFormFile? Logo { get; set; }
    }
}
