using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalMVC.Models
{
    public class SkillModel
    {
        [Key]
        public int SkillId { get; set; }

        [Required(ErrorMessage = "Skill name is required.")]
        [StringLength(100, ErrorMessage = "Skill name cannot be longer than 100 characters.")]
        [Display(Name = "Skill Name")]
        public string SkillName { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int UserId { get; set; }
    }
}