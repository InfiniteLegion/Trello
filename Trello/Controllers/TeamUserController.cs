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

        [HttpPost]
        public async Task<ActionResult> AddUserToTeam(TeamUser teamUser)
        {
            if (teamUser == null)
            {
                return BadRequest("TeamUser object is null");
            }

            await db.TeamUsers.AddAsync(teamUser);
            await db.SaveChangesAsync();
            return Ok("User added to team");
        }

        [HttpDelete("team={teamId}&user={userId}")]
        public async Task<ActionResult> DeleteUserFromTeam(int teamId, int userId)
        {
            TeamUser teamUser = await db.TeamUsers.FirstOrDefaultAsync(x => x.IdTeam == teamId && x.IdUser == userId);

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
