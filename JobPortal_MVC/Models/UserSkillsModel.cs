namespace JobPortalMVC.Models
{
    public class UserSkillsModel
    {
        public int UserSkillId { get; set; }

        public int UserId { get; set; }

        public int SkillId { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
