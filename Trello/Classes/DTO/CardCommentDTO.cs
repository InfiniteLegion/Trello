using Trello.Models;

namespace Trello.Classes.DTO
{
    public class CardCommentDTO
    {
        public long Id { get; set; }

        public string? CommentText { get; set; }

        public DateTime? CommentDatetime { get; set; }

        public long? IdCard { get; set; }

        public UserDto? User { get; set; }
    }
}
