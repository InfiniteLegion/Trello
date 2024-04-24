using System;
using System.Collections.Generic;

namespace Trello;

public partial class TeamUser
{
    public long Id { get; set; }

    public long IdTeam { get; set; }

    public long IdUser { get; set; }

    public virtual Team IdTeamNavigation { get; set; } = null!;

    public virtual UserInfo IdUserNavigation { get; set; } = null!;
}
