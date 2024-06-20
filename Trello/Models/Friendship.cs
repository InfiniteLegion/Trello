using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class Friendship
{
    public long Id { get; set; }

    public long? IdUser1Sender { get; set; }

    public long? IdUser2Receiver { get; set; }

    public string? Status { get; set; }
    [JsonIgnore]
    public virtual UserInfo? IdUser1SenderNavigation { get; set; }
    [JsonIgnore]
    public virtual UserInfo? IdUser2ReceiverNavigation { get; set; }
}
