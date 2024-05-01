using Trello.Models;

namespace Trello.Classes
{
    public class CardValidator
    {
        public static void CheckCardUpdate(Card cardToUpdate, Card originalCard)
        {
            if (cardToUpdate.Title != null)
            {
                originalCard.Title = cardToUpdate.Title;
            }
            if (cardToUpdate.Label != null)
            {
                originalCard.Label = cardToUpdate.Label;
            }
            if (cardToUpdate.StartDate != null)
            {
                originalCard.StartDate = cardToUpdate.StartDate;
            }
            if (cardToUpdate.Deadline != null)
            {
                originalCard.Deadline = cardToUpdate.Deadline;
            }
            if (cardToUpdate.IdStatus != null)
            {
                originalCard.IdStatus = cardToUpdate.IdStatus;
            }
            if (cardToUpdate.IdBoard != null)
            {
                originalCard.IdBoard = cardToUpdate.IdBoard;
            }
        }
    }
}
