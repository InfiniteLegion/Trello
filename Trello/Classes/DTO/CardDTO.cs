namespace Trello.Classes.DTO
{
    public class CardDTO
    {
        public long Id { get; set; }

        public string? Title { get; set; }

        public string? Label { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? Deadline { get; set; }

        public long? IdStatus { get; set; }

        public long? IdBoard { get; set; }

        public List<CardCommentDTO>? CardCommentDTOs { get; set; }

        public List<TaskDTO>? TaskDTOs { get; set; }

        public List<TagDTO>? TagDTOs { get; set; }

        public List<UserDto>? UserDtos { get; set; }
    }
}
