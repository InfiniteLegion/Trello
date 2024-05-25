using System.Text.Json.Serialization;

namespace Trello.Classes.DTO
{
    public partial class UserDto
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Guid { get; set; }
    }
}
