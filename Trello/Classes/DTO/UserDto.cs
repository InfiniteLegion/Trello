namespace Trello.Classes.DTO
{
    public partial class UserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
    }
}
