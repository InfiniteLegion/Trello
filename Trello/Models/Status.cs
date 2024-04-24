using System;
using System.Collections.Generic;

namespace Trello;

public partial class Status
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
}
