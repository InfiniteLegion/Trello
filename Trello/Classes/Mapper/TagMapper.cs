using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Classes.Mapper
{
    public class TagMapper
    {
        public async Task<TagDTO> ToDTO(Tag tag)
        {
            return new TagDTO()
            {
                Id = tag.Id,
                Name = tag.Name,
                IdBoard = tag.IdBoard
            };
        }
    }
}
