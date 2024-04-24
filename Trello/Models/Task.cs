using System;
using System.Collections.Generic;

namespace Trello;

public partial class Task
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public bool? Isactive { get; set; }

    public long? IdCard { get; set; }

    public virtual Card? IdCardNavigation { get; set; }
}
