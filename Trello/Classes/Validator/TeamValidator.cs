namespace Trello.Classes
{
    public class TeamValidator
    {
        public static void CheckTeamUpdate(Team teamToUpdate, Team originalTeam)
        {
            if (teamToUpdate.Name != null)
            {
                originalTeam.Name = teamToUpdate.Name;
            }
        }
    }
}
