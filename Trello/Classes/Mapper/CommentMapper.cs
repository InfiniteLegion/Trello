using Microsoft.EntityFrameworkCore;
using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Classes.Mapper
{
    public class CommentMapper
    {
        private CheloDbContext db;
        private UserMapper userMapper;

        public CommentMapper(CheloDbContext db, UserMapper userMapper) 
        { 
            this.db = db; 
            this.userMapper = userMapper;
        }

        public async Task<CardCommentDTO> ToDTO(CardComment comment)
        {
            if (comment == null)
            {
                return null;
            }

            CardCommentDTO commentDTO = new CardCommentDTO()
            {
                Id = comment.Id,
                CommentText = comment.CommentText,
                CommentDatetime = comment.CommentDatetime,
                IdCard = comment.IdCard
            };

            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(comment.GuidUser));
            UserDTO userDto = await userMapper.ToDTO(user);

            commentDTO.User = userDto;

            return commentDTO;
        }
    }
}
