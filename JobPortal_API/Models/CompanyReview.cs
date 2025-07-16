using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JobPortalAPI.Models;

public partial class CompanyReview
{
    public int ReviewId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual Company? Company { get; set; } = null!;

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
