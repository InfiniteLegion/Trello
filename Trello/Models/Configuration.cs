using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class Configuration
{
    public long Id { get; set; }

    public string? GuidUser { get; set; }
    public bool? IsprivateTeamNotifications { get; set; }
    [JsonIgnore]
    public virtual UserInfo? GuidUserNavigation { get; set; }
}
