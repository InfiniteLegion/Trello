using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Classes;
using Trello.Models;

namespace Trello.Controllers
{
    [ApiController]
    [Route("api/teams")]
    public class TeamController : ControllerBase
    {
        private CheloDbContext db;

        public TeamController(CheloDbContext db) { this.db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetAllTeams()
        {
            return await db.Teams.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeamById(int id)
        {
            Team team = await db.Teams.FirstOrDefaultAsync(x => x.Id == id);

            if (team == null)
            {
                return BadRequest("Team not found");
            }

            return new ObjectResult(team);
        }

        [HttpPost("user={userGuid}")]
        public async Task<ActionResult<Team>> AddTeam(Team team, string userGuid)
        {
            if (team == null)
            {
                return BadRequest("Team is null");
            }

            UserInfo user = await db.UserInfos
                .FirstOrDefaultAsync(x => x.Guid.Equals(userGuid));

            await db.Teams.AddAsync(team);
            await db.SaveChangesAsync();

            Team lastTeam = await db.Teams
                .OrderBy(x => x.Id)
                .LastAsync();

            TeamUser teamUser = new TeamUser() { IdTeam = lastTeam.Id, IdUser = user.Id, Role = "ADMIN" };

            await db.TeamUsers.AddAsync(teamUser);
            await db.SaveChangesAsync();

            return Ok(team);
        }

        [HttpPut("isAdmin={userGuid}")]
        public async Task<ActionResult<Team>> UpdateTeam(Team team, string userGuid)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(userGuid));
            TeamUser teamUser = await db.TeamUsers.FirstOrDefaultAsync(x => x.IdTeam == team.Id && x.IdUser == user.Id);

            if (teamUser.Role.Equals("ADMIN"))
            {
                if (team == null)
                {
                    return BadRequest("Team is null");
                }
                if (!db.Teams.Any(x => x.Id == team.Id))
                {
                    return BadRequest("Team not found");
                }

                Team originalTeam = await db.Teams.FirstOrDefaultAsync(x => x.Id == team.Id);

                TeamValidator.CheckTeamUpdate(team, originalTeam);
                await db.SaveChangesAsync();
                return Ok(originalTeam);
            }
            else
            {
                return BadRequest("User is not admin");
            }
        }

        [HttpDelete("{id}&isAdmin={userGuid}")]
        public async Task<ActionResult> DeleteTeamById(int id, string userGuid)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(userGuid));
            TeamUser teamUser = await db.TeamUsers.FirstOrDefaultAsync(x => x.IdTeam == id && x.IdUser == user.Id);

            if (!teamUser.Role.Equals("ADMIN"))
            {
                return BadRequest("User is not admin");
            }

            Team team = await db.Teams.FirstOrDefaultAsync(x => x.Id == id);

            if (team == null)
            {
                return BadRequest("Team not found");
            }

            var teamUsers = await db.TeamUsers
                .Where(x => x.Id == team.Id)
                .ToListAsync();

            foreach (var item in teamUsers)
            {
                db.TeamUsers.Remove(item);
            }

            var teamBoards = await db.Boards
                .Where(x => x.IdTeam == team.Id)
                .ToListAsync();

            foreach (var item in teamBoards)
            {
                db.Boards.Remove(item);
            }

            db.Teams.Remove(team);
            await db.SaveChangesAsync();
            return Ok("Team deleted");
        }

        [HttpGet("{id}/users")]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetAllTeamMembers(int id)
        {
            Team team = await db.Teams.FirstOrDefaultAsync(x => x.Id == id);

            if (team == null)
            {
                return BadRequest("Team not found");
            }

            var teamUsers = await db.TeamUsers.Where(x => x.IdTeam == id).ToListAsync();
            var users = new List<UserInfo>();
            foreach (var item in teamUsers)
            {
                users.Add(await db.UserInfos.FirstOrDefaultAsync(x => x.Id == item.IdUser));
            }

            return users;
        }
    }
}
