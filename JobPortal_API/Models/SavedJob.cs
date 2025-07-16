using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JobPortalAPI.Models;

public partial class SavedJob
{
    public int SavedJobId { get; set; }

    public int UserId { get; set; }

    public int JobId { get; set; }

    public DateTime? SavedAt { get; set; }

    [JsonIgnore]
    public virtual Job? Job { get; set; } = null!;

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
