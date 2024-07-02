using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class CardComment
{
    public long Id { get; set; }

    public string? CommentText { get; set; }

    public DateTime? CommentDatetime { get; set; }

    public long? IdCard { get; set; }

    public string? GuidUser { get; set; }
    [JsonIgnore]
    public virtual UserInfo? GuidUserNavigation { get; set; }
    [JsonIgnore]
    public virtual Card? IdCardNavigation { get; set; }
}
