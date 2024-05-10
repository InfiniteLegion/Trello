using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class StatusColumn
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<BoardStatusColumn> BoardStatusColumns { get; set; } = new List<BoardStatusColumn>();

    [JsonIgnore]
    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
}
