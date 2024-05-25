namespace Trello.Classes.DTO
{
    public class TaskDTO
    {
        public long Id { get; set; }

        public string? Title { get; set; }

        public bool? Iscompleted { get; set; }

        public long? IdCard { get; set; }
    }
}
