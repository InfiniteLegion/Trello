using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class TeamUserNotification
{
    public long Id { get; set; }

    public long? IdSender { get; set; }

    public long? IdReceiver { get; set; }

    public string? Status { get; set; }
    [JsonIgnore]
    public virtual UserInfo? IdReceiverNavigation { get; set; }
    [JsonIgnore]
    public virtual UserInfo? IdSenderNavigation { get; set; }
}
