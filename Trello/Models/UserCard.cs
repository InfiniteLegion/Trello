using System;
using System.Collections.Generic;

namespace Trello;

public partial class UserCard
{
    public long Id { get; set; }

    public long IdUser { get; set; }

    public long IdCard { get; set; }

    public virtual Card IdCardNavigation { get; set; } = null!;

    public virtual UserInfo IdUserNavigation { get; set; } = null!;
}
