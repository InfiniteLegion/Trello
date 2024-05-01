using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Trello.Models;

namespace Trello;

public partial class Status
{
    public long Id { get; set; }

    public string? Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
}
