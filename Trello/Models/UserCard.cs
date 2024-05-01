using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class UserCard
{
    public long Id { get; set; }

    public long IdUser { get; set; }

    public long IdCard { get; set; }
    [JsonIgnore]
    public virtual Card IdCardNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual UserInfo IdUserNavigation { get; set; } = null!;
}
