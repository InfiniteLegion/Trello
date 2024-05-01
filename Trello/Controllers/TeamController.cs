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

        [HttpPost]
        public async Task<ActionResult<Team>> AddTeam(Team team)
        {
            if (team == null)
            {
                return BadRequest("Team is null");
            }

            await db.Teams.AddAsync(team);
            await db.SaveChangesAsync();
            return Ok(team);
        }

        [HttpPut]
        public async Task<ActionResult<Team>> UpdateTeam(Team team)
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeamById(int id)
        {
            Team team = await db.Teams.FirstOrDefaultAsync(x => x.Id == id);

            if (team == null)
            {
                return BadRequest("Team not found");
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
