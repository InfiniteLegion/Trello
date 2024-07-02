using Microsoft.EntityFrameworkCore;
using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Classes.Mapper
{
    public class TeamNotificationMapper
    {
        private CheloDbContext db;

        public TeamNotificationMapper(CheloDbContext db)
        {
            this.db = db;
        }

        public async Task<TeamNotificationDTO> ToDTO(TeamUserNotification notification)
        {
            if (notification == null)
            {
                return null;
            }

            Team team = await db.Teams.FirstOrDefaultAsync(x => x.Id == notification.IdSender);

            return new TeamNotificationDTO
            {
                Id = notification.Id,
                TeamName = team.Name
            };
        }
    }
}
