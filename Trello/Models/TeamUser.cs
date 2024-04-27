using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello;

public partial class TeamUser
{
    public long Id { get; set; }

    public long IdTeam { get; set; }

    public long IdUser { get; set; }

    [JsonIgnore]
    public virtual Team? IdTeamNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual UserInfo? IdUserNavigation { get; set; } = null!;
}
