using System;
using System.Collections.Generic;

namespace Trello;

public partial class Board
{
    public long Id { get; set; }

    public string? Name { get; set; } = null!;

    public long? IdTeam { get; set; }

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    public virtual Team? IdTeamNavigation { get; set; }
}
