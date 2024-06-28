using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using Trello.Classes.DTO;
using Trello.Classes.Mapper;
using Trello.Models;

namespace Trello.Controllers
{
    [Route("api/team-user-notifications")]
    [ApiController]
    public class TeamUserNotificationController : ControllerBase
    {
        private CheloDbContext db;
        private readonly TeamNotificationMapper teamNotificationMapper;

        public TeamUserNotificationController(CheloDbContext db, TeamNotificationMapper teamNotificationMapper)
        {
            this.db = db;
            this.teamNotificationMapper = teamNotificationMapper;
        }

        [HttpGet("user={guid}")]
        public async Task<ActionResult<List<TeamNotificationDTO>>> GetTeamUserNotificationsByUserGuid(string guid)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(guid));
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var teamNotifications = await db.TeamUserNotifications
                .Where(x => x.IdReceiver == user.Id)
                .ToListAsync();

            var teamDTOs = new List<TeamNotificationDTO>();
            foreach (var teamNotification in teamNotifications)
            {
                TeamNotificationDTO teamNotificationDTO = await teamNotificationMapper.ToDTO(teamNotification);
                teamDTOs.Add(teamNotificationDTO);
            }

            return Ok(teamDTOs);
        }

        [HttpPut("notification={notificationId}&decision={decision}")]
        public async Task<ActionResult> AcceptDenyInvitation(int notificationId, string decision)
        {
            TeamUserNotification notification = await db.TeamUserNotifications
                .FirstOrDefaultAsync(x => x.Id == notificationId);

            if (notification == null)
            {
                return BadRequest("Wrong id");
            }

            if (decision.ToLower().Equals("accept"))
            {
                TeamUser teamUser = new TeamUser() { IdTeam = notification.IdSender, IdUser = notification.IdReceiver, Role = "USER" };

                await db.TeamUsers.AddAsync(teamUser);
                db.TeamUserNotifications.Remove(notification);
                await db.SaveChangesAsync();

                return Ok("User added to team");
            }
            else if (decision.ToLower().Equals("deny"))
            {
                db.TeamUserNotifications.Remove(notification);
                await db.SaveChangesAsync();
                return Ok("Invitation denied");
            }
            else
            {
                return Ok("Wrong command");
            }
        }
    }
}
