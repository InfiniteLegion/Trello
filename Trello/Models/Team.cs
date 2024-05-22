using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class Team
{
    public long Id { get; set; }

    public string? Name { get; set; }
    [JsonIgnore]
    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();
    [JsonIgnore]
    public virtual ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
}
