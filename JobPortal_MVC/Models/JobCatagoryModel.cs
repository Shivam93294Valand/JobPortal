using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalMVC.Models
{
    public class JobCatagoryModel
    {
        [Key] 
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")] 
        [StringLength(100)]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        public int UserId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
