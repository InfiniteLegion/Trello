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

        public CardMapper(CheloDbContext db, CommentMapper commentMapper, TagMapper tagMapper) 
        { 
            this.db = db; 
            this.commentMapper = commentMapper;
            this.tagMapper = tagMapper;
        }

        public async Task<CardDTO> ToDTO(Card card)
        {
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

            cardDTO.TaskDTOs = taskDTOs;
            cardDTO.TagDTOs = tagDTOs;
            cardDTO.CardCommentDTOs = commentDTOs;

            return cardDTO;
        }
    }
}
