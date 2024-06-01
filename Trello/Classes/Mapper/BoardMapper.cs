using Microsoft.EntityFrameworkCore;
using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Classes.Mapper
{
    public class BoardMapper
    {
        private CheloDbContext db;
        private readonly UserMapper userMapper;
        private readonly StatusColumnMapper statusColumnMapper;
        private readonly TagMapper tagMapper;
        private readonly CardMapper cardMapper;

        public BoardMapper(CheloDbContext db, UserMapper userMapper, StatusColumnMapper statusColumnMapper, TagMapper tagMapper, CardMapper cardMapper)
        {
            this.db = db;
            this.userMapper = userMapper;
            this.statusColumnMapper = statusColumnMapper;
            this.tagMapper = tagMapper;
            this.cardMapper = cardMapper;
        }

        public async Task<BoardDTO> ToDTO(Board board)
        {
            BoardDTO boardDTO = new BoardDTO()
            {
                Id = board.Id,
                Name = board.Name,
                IdTeam = board.IdTeam
            };

            var teamUsers = await db.TeamUsers.Where(x => x.IdTeam == boardDTO.IdTeam).ToListAsync();
            var users = new List<UserInfo>();
            foreach (var item in teamUsers)
            {
                users.Add(await db.UserInfos.FirstOrDefaultAsync(x => x.Id == item.IdUser));
            }

            var userDTOs = new List<UserDTO>();
            foreach (var item in users)
            {
                UserDTO userDTO = await userMapper.ToDTO(item);
                userDTOs.Add(userDTO);
            }

            var statusColumns = await db.StatusColumns.Where(x => x.IdBoard == boardDTO.Id).ToListAsync();
            var statusColumnsDTOs = new List<StatusColumnDTO>();
            foreach (var item in statusColumns)
            {
                StatusColumnDTO statusColumnDTO = await statusColumnMapper.ToDTO(item);
                statusColumnsDTOs.Add(statusColumnDTO);
            }

            var tags = await db.Tags.Where(x => x.IdBoard == boardDTO.Id).ToListAsync();
            var tagDTOs = new List<TagDTO>();
            foreach (var item in tags)
            {
                TagDTO tagDTO = await tagMapper.ToDTO(item);
                tagDTOs.Add(tagDTO);
            }

            var cards = await db.Cards.Where(x => x.IdBoard == boardDTO.Id).ToListAsync();
            var cardsDTOs = new List<CardDTO>();
            foreach (var item in cards)
            {
                CardDTO cardDTO = await cardMapper.ToDTO(item);
                cardsDTOs.Add(cardDTO);
            }

            boardDTO.Users = userDTOs;
            boardDTO.StatusColumns = statusColumnsDTOs;
            boardDTO.Tags = tagDTOs;
            boardDTO.Cards = cardsDTOs;

            return boardDTO;
        }
    }
}
