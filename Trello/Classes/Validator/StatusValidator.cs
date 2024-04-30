namespace Trello.Classes
{
    public class StatusValidator
    {
        public static void CheckStatusUpdate(Status statusToUpdate, Status originalStatus)
        {
            if (statusToUpdate.Name != null)
            {
                originalStatus.Name = statusToUpdate.Name;
            }
        }
    }
}
