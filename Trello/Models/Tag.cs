using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class Tag
{
    public long Id { get; set; }

    public string? Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<BoardTag> BoardTags { get; set; } = new List<BoardTag>();

    [JsonIgnore]
    public virtual ICollection<CardTag> CardTags { get; set; } = new List<CardTag>();
}
