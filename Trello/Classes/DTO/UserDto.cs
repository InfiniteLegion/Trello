using System.Text.Json.Serialization;

namespace Trello.Classes.DTO
{
    public partial class UserDTO
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Guid { get; set; }
        public ConfigurationDTO ConfigurationDTO { get; set; }
    }
}
