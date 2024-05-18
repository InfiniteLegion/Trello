using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class Board
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? IdTeam { get; set; }
    [JsonIgnore]
    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    [JsonIgnore]
    public virtual Team? IdTeamNavigation { get; set; }
    [JsonIgnore]
    public virtual ICollection<StatusColumn> StatusColumns { get; set; } = new List<StatusColumn>();
    [JsonIgnore]
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
