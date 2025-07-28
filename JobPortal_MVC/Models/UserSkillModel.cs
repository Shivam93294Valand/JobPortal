namespace JobPortalMVC.Models
{
    public class UserSkillModel
    {
        public int UserSkillId { get; set; }
        public int UserId { get; set; }
        public int SkillId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? SkillName { get; set; }
        public string? UserName { get; set; }
    }
}
