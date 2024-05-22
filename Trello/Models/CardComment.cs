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

    public long? IdUser { get; set; }
    [JsonIgnore]
    public virtual Card? IdCardNavigation { get; set; }
    [JsonIgnore]
    public virtual UserInfo? IdUserNavigation { get; set; }
}
