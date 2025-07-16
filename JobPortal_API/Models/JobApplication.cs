using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JobPortalAPI.Models;

public partial class JobApplication
{
    public int JobApplicationId { get; set; }

    public int JobId { get; set; }

    public int UserId { get; set; }

    public string ResumeUrl { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? AppliedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual Job? Job { get; set; } = null!;

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
