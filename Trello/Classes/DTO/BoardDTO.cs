using Trello.Models;

namespace Trello.Classes.DTO
{
    public class BoardDTO
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public long? IdTeam { get; set; }

        public List<UserDTO>? Users { get; set; }

        public List<StatusColumnDTO>? StatusColumns { get; set; }

        public List<TagDTO>? Tags { get; set; }

        public List<CardDTO>? Cards { get; set; }
    }
}
