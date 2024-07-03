using Microsoft.EntityFrameworkCore;
using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Classes.Mapper
{
    public class CardMapper
    {
        private CheloDbContext db;
        private readonly CommentMapper commentMapper;
        private readonly TagMapper tagMapper;
        private readonly UserMapper userMapper;

        public CardMapper(CheloDbContext db, CommentMapper commentMapper, TagMapper tagMapper, UserMapper userMapper) 
        { 
            this.db = db; 
            this.commentMapper = commentMapper;
            this.tagMapper = tagMapper;
            this.userMapper = userMapper;
        }

        public async Task<CardDTO> ToDTO(Card card)
        {
            if (card == null)
            {
                return null;
            }

            CardDTO cardDTO = new CardDTO()
            {
                Id = card.Id,
                Title = card.Title,
                Label = card.Label,
                StartDate = card.StartDate,
                Deadline = card.Deadline,
                IdStatus = card.IdStatus,
                IdBoard = card.IdBoard
            };

            var tasks = await db.Tasks.Where(x => x.IdCard == cardDTO.Id).ToListAsync();
            var taskDTOs = new List<TaskDTO>();

            foreach (var task in tasks)
            {
                taskDTOs.Add(TaskMapper.ToDTO(task));
            }

            var cardTags = await db.CardTags.Where(x => x.IdCard == cardDTO.Id).ToListAsync();
            var tagDTOs = new List<TagDTO>();
            foreach (var item in cardTags)
            {
                Tag? tag = await db.Tags.FirstOrDefaultAsync(x => x.Id == item.IdTags);
                tagDTOs.Add(await tagMapper.ToDTO(tag));
            }

            var comments = await db.CardComments.Where(x => x.IdCard == card.Id).ToListAsync();
            var commentDTOs = new List<CardCommentDTO>();
            foreach (var item in comments)
            {
                CardCommentDTO commentDTO = await commentMapper.ToDTO(item);
                commentDTOs.Add(commentDTO);
            }

            var userCards = await db.UserCards.Where(x => x.IdCard == card.Id).ToListAsync();
            var users = new List<UserInfo>();
            foreach (var item in userCards)
            {
                UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(item.GuidUser));
                users.Add(user);
            }

            var userDTOs = new List<UserDTO>();
            foreach (var item in users)
            {
                UserDTO userDTO = await userMapper.ToDTO(item);
                userDTOs.Add(userDTO);
            }

            cardDTO.TaskDTOs = taskDTOs;
            cardDTO.TagDTOs = tagDTOs;
            cardDTO.CardCommentDTOs = commentDTOs;
            cardDTO.UserDtos = userDTOs;

            return cardDTO;
        }
    }
}
