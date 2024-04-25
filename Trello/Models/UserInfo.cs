using System;
using System.Collections.Generic;

namespace Trello;

public partial class UserInfo
{
    public long Id { get; set; }

    public string? Username { get; set; } = null!;

    public string? Email { get; set; } = null!;

    public string? Password { get; set; } = null!;

    public string? Role { get; set; }

    public virtual ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();

    public virtual ICollection<UserCard> UserCards { get; set; } = new List<UserCard>();
}
