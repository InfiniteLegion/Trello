using System;
using System.Collections.Generic;

namespace Trello;

public partial class Card
{
    public long Id { get; set; }

    public string? Title { get; set; } = null!;

    public string? Label { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? Deadline { get; set; }

    public long? IdStatus { get; set; }

    public long? IdBoard { get; set; }

    public virtual Board? IdBoardNavigation { get; set; } = null!;

    public virtual Status? IdStatusNavigation { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<UserCard> UserCards { get; set; } = new List<UserCard>();
}
