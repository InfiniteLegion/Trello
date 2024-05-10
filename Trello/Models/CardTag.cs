using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class CardTag
{
    public long Id { get; set; }

    public long IdCard { get; set; }

    public long IdTags { get; set; }

    [JsonIgnore]
    public virtual Card IdCardNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual Tag IdTagsNavigation { get; set; } = null!;
}
