using Trello.Classes.DTO;

namespace Trello.Classes.Mapper
{
    public class TaskMapper
    {
        public static TaskDTO ToDTO(Models.Task task)
        {
            return new TaskDTO()
            {
                Id = task.Id,
                Title = task.Title,
                Iscompleted = task.Iscompleted,
                IdCard = task.IdCard
            };
        }
    }
}
