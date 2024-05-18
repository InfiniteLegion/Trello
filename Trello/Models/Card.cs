using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class Card
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Label { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? Deadline { get; set; }

    public long? IdStatus { get; set; }

    public long? IdBoard { get; set; }
    [JsonIgnore]
    public virtual ICollection<CardTag> CardTags { get; set; } = new List<CardTag>();
    [JsonIgnore]
    public virtual Board? IdBoardNavigation { get; set; }
    [JsonIgnore]
    public virtual StatusColumn? IdStatusNavigation { get; set; }
    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
    [JsonIgnore]
    public virtual ICollection<UserCard> UserCards { get; set; } = new List<UserCard>();
}
