using Trello.Models;

namespace Trello.Classes
{
    public class StatusValidator
    {
        public static void CheckStatusUpdate(StatusColumn statusToUpdate, StatusColumn originalStatus)
        {
            if (statusToUpdate.Name != null)
            {
                originalStatus.Name = statusToUpdate.Name;
            }
            if (statusToUpdate.IdBoard != null)
            {
                originalStatus.IdBoard = statusToUpdate.IdBoard;
            }
        }
    }
}
