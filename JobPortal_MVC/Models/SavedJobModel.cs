namespace JobPortalMVC.Models
{
    public class SavedJobModel
    {
        public int SavedJobId { get; set; }

        public int UserId { get; set; }

        public int JobId { get; set; }

        public DateTime? SavedAt { get; set; }
    }
}
