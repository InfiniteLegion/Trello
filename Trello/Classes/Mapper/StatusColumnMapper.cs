using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Classes.Mapper
{
    public class StatusColumnMapper
    {
        public async Task<StatusColumnDTO> ToDTO(StatusColumn statusColumn)
        {
            if (statusColumn == null)
            {
                return null;
            }

            return new StatusColumnDTO()
            {
                Id = statusColumn.Id,
                Name = statusColumn.Name,
                IdBoard = statusColumn.IdBoard
            };
        }
    }
}
