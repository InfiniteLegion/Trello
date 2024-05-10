using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class BoardTag
{
    public long Id { get; set; }

    public long IdBoard { get; set; }

    public long IdTags { get; set; }

    [JsonIgnore]
    public virtual Board IdBoardNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual Tag IdTagsNavigation { get; set; } = null!;
}
