using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class BoardStatusColumn
{
    public long Id { get; set; }

    public long? IdBoard { get; set; }

    public long? IdStatusColumn { get; set; }
    [JsonIgnore]
    public virtual Board? IdBoardNavigation { get; set; }
    [JsonIgnore]
    public virtual StatusColumn? IdStatusColumnNavigation { get; set; }
}
