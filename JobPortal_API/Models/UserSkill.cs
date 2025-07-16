using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JobPortalAPI.Models;

public partial class UserSkill
{
    public int UserSkillId { get; set; }

    public int UserId { get; set; }

    public int SkillId { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public virtual Skill? Skill { get; set; } = null!;

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
