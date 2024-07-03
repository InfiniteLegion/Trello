using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Models;

namespace Trello.Controllers
{
    [Route("api/team-user")]
    [ApiController]
    public class TeamUserController : ControllerBase
    {
        private CheloDbContext db;
        

        public TeamUserController(CheloDbContext db) { this.db = db; }

        [HttpPost("add/team={teamId}&user={userGuid}&isAdmin={isAdminGuid}")]
        public async Task<ActionResult> AddUserToTeam(long teamId, string userGuid, string isAdminGuid)
        {
            UserInfo isAdminUser = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(isAdminGuid));
            TeamUser teamUser1 = await db.TeamUsers.FirstOrDefaultAsync(x => x.IdTeam == teamId && x.IdUser == isAdminUser.Id);

            if (!teamUser1.Role.Equals("ADMIN"))
            {
                return BadRequest("User is not admin");
            }

            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(userGuid));
            Team team = await db.Teams.FirstOrDefaultAsync(x => x.Id == teamId);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (team == null)
            {
                return BadRequest("Team not found");
            }

            if (await db.TeamUsers.FirstOrDefaultAsync(x => x.IdTeam == team.Id && x.IdUser == user.Id) != null)
            {
                return BadRequest("This user already in this team");
            }

            if (await db.TeamUserNotifications.FirstOrDefaultAsync(x => x.IdSender == team.Id && x.IdReceiver == user.Id) != null)
            {
                return BadRequest("User already have an invitation from this team");
            }

            Configuration configuration = await db.Configurations.FirstOrDefaultAsync(x => x.GuidUser.Equals(user.Guid));

            if ((bool)configuration.IsprivateTeamNotifications)
            {
                TeamUserNotification notification = new TeamUserNotification() { IdSender = team.Id, IdReceiver = user.Id, Status = "WAITING" };
                await db.TeamUserNotifications.AddAsync(notification);
                await db.SaveChangesAsync();
                return Ok("Invitation sent!");
            }
            else
            {
                TeamUser teamUser = new TeamUser { IdTeam = team.Id, IdUser = user.Id };

                await db.TeamUsers.AddAsync(teamUser);
                await db.SaveChangesAsync();

                return Ok("User added to team");
            }
        }

        [HttpDelete("team={teamId}&user={userGuid}&isAdmin={isAdminGuid}")]
        public async Task<ActionResult> DeleteUserFromTeam(int teamId, string userGuid, string isAdminGuid)
        {
            UserInfo isAdminUser = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(isAdminGuid));
            TeamUser teamUser1 = await db.TeamUsers.FirstOrDefaultAsync(x => x.IdTeam == teamId && x.IdUser == isAdminUser.Id);

            if (!teamUser1.Role.Equals("ADMIN"))
            {
                return BadRequest("User is not admin");
            }

            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(userGuid));
            TeamUser teamUser = await db.TeamUsers.FirstOrDefaultAsync(x => x.IdTeam == teamId && x.IdUser == user.Id);

            if (teamUser == null)
            {
                return BadRequest("TeamUser object not found");
            }

            db.TeamUsers.Remove(teamUser);
            await db.SaveChangesAsync();
            return Ok("User deleted from team");
        }
    }
}
