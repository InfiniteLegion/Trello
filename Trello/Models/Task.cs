using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello;

public partial class Task
{
    public long Id { get; set; }

    public string? Title { get; set; } = null!;

    public bool? Iscompleted { get; set; } = false;

    public long? IdCard { get; set; }
    [JsonIgnore]
    public virtual Card? IdCardNavigation { get; set; }
}
