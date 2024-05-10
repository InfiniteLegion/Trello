using Trello.Models;

namespace Trello.Classes.Validator
{
    public class BoardValidator
    {
        public static void CheckBoardUpdate(Board boardToUpdate, Board originalBoard)
        {
            if (boardToUpdate.Name != null)
            {
                originalBoard.Name = boardToUpdate.Name;
            }

            if (boardToUpdate.IdTeam != null)
            {
                originalBoard.IdTeam = boardToUpdate.IdTeam;
            }
        }
    }
}
