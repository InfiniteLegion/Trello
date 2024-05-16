using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class Tag
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public long? IdBoard { get; set; }
    [JsonIgnore]
    public virtual ICollection<CardTag> CardTags { get; set; } = new List<CardTag>();
    [JsonIgnore]
    public virtual Board? IdBoardNavigation { get; set; }
}
