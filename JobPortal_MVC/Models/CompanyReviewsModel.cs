namespace JobPortalMVC.Models
{
    public class CompanyReviewsModel
    {
        public int ReviewId { get; set; }

        public int CompanyId { get; set; }

        public int UserId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
