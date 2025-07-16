using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JobPortalAPI.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Website { get; set; }

    public string? Description { get; set; }

    public string? LogoUrl { get; set; }

    public int UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<CompanyReview> CompanyReviews { get; set; } = new List<CompanyReview>();

    [JsonIgnore]
    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
