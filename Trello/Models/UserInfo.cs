using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello.Models;

public partial class UserInfo
{
    public long Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Guid { get; set; }
    [JsonIgnore]
    public virtual ICollection<CardComment> CardComments { get; set; } = new List<CardComment>();
    [JsonIgnore]
    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();
    [JsonIgnore]
    public virtual ICollection<Friendship> FriendshipIdUser1SenderNavigations { get; set; } = new List<Friendship>();
    [JsonIgnore]
    public virtual ICollection<Friendship> FriendshipIdUser2ReceiverNavigations { get; set; } = new List<Friendship>();
    [JsonIgnore]
    public virtual ICollection<TeamUserNotification> TeamUserNotificationIdReceiverNavigations { get; set; } = new List<TeamUserNotification>();
    [JsonIgnore]
    public virtual ICollection<TeamUserNotification> TeamUserNotificationIdSenderNavigations { get; set; } = new List<TeamUserNotification>();
    [JsonIgnore]
    public virtual ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
    [JsonIgnore]
    public virtual ICollection<UserCard> UserCards { get; set; } = new List<UserCard>();
}
