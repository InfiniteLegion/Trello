using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Models;

namespace Trello.Controllers
{
    [Route("api/user-card")]
    [ApiController]
    public class UserCardController : ControllerBase
    {
        private CheloDbContext db;

        public UserCardController(CheloDbContext db) { this.db = db; }

        [HttpPost]
        public async Task<ActionResult> AddUserToCard(UserCard userCard)
        {
            if (userCard == null)
            {
                return BadRequest("UserCard object is null");
            }

            await db.UserCards.AddAsync(userCard);
            await db.SaveChangesAsync();
            return Ok("User added to card");
        }

        [HttpDelete("card={cardId}&user={userId}")]
        public async Task<ActionResult> DeleteUserFromTeam(int cardId, int userId)
        {
            UserCard userCard = await db.UserCards.FirstOrDefaultAsync(x => x.IdCard == cardId && x.IdUser == userId);

            if (userCard == null)
            {
                return BadRequest("UserCard object not found");
            }

            db.UserCards.Remove(userCard);
            await db.SaveChangesAsync();
            return Ok("User deleted from card");
        }
    }
}
