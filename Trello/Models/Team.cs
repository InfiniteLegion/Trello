using System;
using System.Collections.Generic;

namespace Trello;

public partial class Team
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();

    public virtual ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
}
