using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JobPortalAPI.Models;

public partial class Skill
{
    public int SkillId { get; set; }

    public string SkillName { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
}
