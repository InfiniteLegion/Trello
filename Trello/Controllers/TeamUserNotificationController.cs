using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using Trello.Models;

namespace Trello.Controllers
{
    [Route("api/teamusernotifications")]
    [ApiController]
    public class TeamUserNotificationController : ControllerBase
    {
        private CheloDbContext db;

        public TeamUserNotificationController(CheloDbContext db) { this.db = db; }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamUserNotification>>> GetAllTeamUserNotifications()
        {
            return await db.TeamUserNotifications.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamUserNotification>> GetTeamUserNotificationsById(int id)
        {
            TeamUserNotification teamUserNotification = await db.TeamUserNotifications.FirstOrDefaultAsync(x => x.Id == id);
            if(teamUserNotification == null)
            {
                return BadRequest("TeamUserNotification object not found");
            }
            return new ObjectResult(teamUserNotification);
        }
        [HttpGet("/{userId}")]
        public ActionResult<IEnumerable<TeamUserNotification>> GetTeamNotifications(int userId)
        {
            var teamUserNotifications = db.TeamUserNotifications.Where(f => f.IdSender == userId || f.IdReceiver == userId).ToList();

            return Ok(teamUserNotifications);
        }
        [HttpPost]
        public async Task<ActionResult<TeamUserNotification>> CreateTeamUserNotifications(TeamUserNotification teamUserNotification)
        {
            if (TeamUserNotificationExists((int)teamUserNotification.IdSender, (int)teamUserNotification.IdReceiver))
            {
                return BadRequest("TeamUserNotification object is null");
            }
            if (teamUserNotification == null)
            {
                return BadRequest("TeamUserNotification object is null");
            }
            await db.TeamUserNotifications.AddAsync(teamUserNotification);
            await db.SaveChangesAsync();
            return Ok(teamUserNotification);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<TeamUserNotification>> DeleteTeamUserNotifications(int id)
        {
            TeamUserNotification teamUserNotification = await db.TeamUserNotifications.FirstOrDefaultAsync(x => x.Id == id);
            if(teamUserNotification == null)
            {
                return BadRequest("TeamUserNotification object is null");
            }
            db.TeamUserNotifications.Remove(teamUserNotification);
            await db.SaveChangesAsync();
            return Ok("TeamUserNotification is deleted!");
        }
        private bool TeamUserNotificationExists(int userId1, int userId2)
        {
            return db.TeamUserNotifications.Any(f => (f.IdSender == userId1 && f.IdReceiver == userId2) || (f.IdSender == userId2 && f.IdReceiver == userId1));
        }
    }
}
