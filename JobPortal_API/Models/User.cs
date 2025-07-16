using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JobPortalAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    [JsonIgnore]
    public virtual ICollection<CompanyReview> CompanyReviews { get; set; } = new List<CompanyReview>();

    [JsonIgnore]
    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    [JsonIgnore]
    public virtual ICollection<JobCategory> JobCategories { get; set; } = new List<JobCategory>();

    [JsonIgnore]
    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    [JsonIgnore]
    public virtual ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();

    [JsonIgnore]
    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();

    [JsonIgnore]
    public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
}
