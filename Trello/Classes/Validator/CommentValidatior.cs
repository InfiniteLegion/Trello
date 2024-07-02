using Trello.Models;

namespace Trello.Classes.Validator
{
    public class CommentValidatior
    {
        public static void CheckCommentUpdate(CardComment commentToUpdate, CardComment originalComment)
        {
            if (commentToUpdate.CommentText != null)
            {
                originalComment.CommentText = commentToUpdate.CommentText;
            }
            if (commentToUpdate.CommentDatetime != null)
            {
                originalComment.CommentDatetime = commentToUpdate.CommentDatetime;
            }
            if (commentToUpdate.IdCard != null)
            {
                originalComment.IdCard = commentToUpdate.IdCard;
            }
            if (commentToUpdate.GuidUser != null)
            {
                originalComment.GuidUser = commentToUpdate.GuidUser;
            }
        }
    }
}
