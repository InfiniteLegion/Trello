namespace Trello.Classes.DTO
{
    public class TeamDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<UserDTO> Users { get; set; }
    }
}
