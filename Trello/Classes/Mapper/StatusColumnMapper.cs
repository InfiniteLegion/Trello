using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Classes.Mapper
{
    public class StatusColumnMapper
    {
        public async Task<StatusColumnDTO> ToDTO(StatusColumn statusColumn)
        {
            return new StatusColumnDTO()
            {
                Id = statusColumn.Id,
                Name = statusColumn.Name,
                IdBoard = statusColumn.IdBoard
            };
        }
    }
}
