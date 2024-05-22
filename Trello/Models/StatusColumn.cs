using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class StatusColumn
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public long? IdBoard { get; set; }
    [JsonIgnore]
    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    [JsonIgnore]
    public virtual Board? IdBoardNavigation { get; set; }
}
