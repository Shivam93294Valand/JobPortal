using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JobPortalAPI.Models;

public partial class JobCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
