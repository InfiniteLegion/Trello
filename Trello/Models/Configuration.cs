using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class Configuration
{
    public long Id { get; set; }

    public long? IdUser { get; set; }

    public bool? IsprivateTeamNotification { get; set; }
    [JsonIgnore]
    public virtual UserInfo? IdUserNavigation { get; set; }
}
