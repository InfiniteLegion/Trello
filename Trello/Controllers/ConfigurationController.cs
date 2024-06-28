using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Models;

namespace Trello.Controllers
{
    [Route("api/config")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly CheloDbContext db;

        public ConfigurationController(CheloDbContext db)
        {
            this.db = db;
        }

        [HttpPut("change/team-notification-privacy/guid={guid}")]
        public async Task<ActionResult> ChangeUserTeamNotificationPrivacy(string guid)
        {
            Configuration configuration = await db.Configurations.FirstOrDefaultAsync(x => x.GuidUser.Equals(guid));

            if (configuration == null)
            {
                return BadRequest("Wrong user guid");
            }

            configuration.IsprivateTeamNotifications = !configuration.IsprivateTeamNotifications;
            await db.SaveChangesAsync();
            return Ok("Team notification privacy changed!");
        }
    }
}