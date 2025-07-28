using System.Text.Json.Serialization;

namespace JobPortalAPI.Models;

public partial class Job
{
    public int JobId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string KeyResponsibilities { get; set; } = null!;

    public string Qualifications { get; set; } = null!;

    public bool IsActive { get; set; }

    public string Location { get; set; } = null!;

    public string? SalaryRange { get; set; }

    public string? ExperienceRange { get; set; }

    public string JobType { get; set; } = null!;

    public DateOnly ExpiresAt { get; set; }

    public int CategoryId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public int SkillId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual JobCategory? Category { get; set; } = null!;

    [JsonIgnore]
    public virtual Company? Company { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    [JsonIgnore]
    public virtual ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();

    [JsonIgnore]
    public virtual Skill? Skill { get; set; } = null!;

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
