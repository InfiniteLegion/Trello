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

        [HttpPost("cardId={cardId}&userGuid={userGuid}")]
        public async Task<ActionResult> AddUserToCard(int cardId, string userGuid)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(userGuid));
            if (user == null)
            {
                return BadRequest("User not found");
            }

            Card card = await db.Cards.FirstOrDefaultAsync(x => x.Id == cardId);
            if (card == null)
            {
                return BadRequest("Card not found");
            }

            UserCard userCard = new UserCard() {  IdCard = card.Id, GuidUser = user.Guid };

            await db.UserCards.AddAsync(userCard);
            await db.SaveChangesAsync();
            return Ok("User added to card");
        }

        [HttpDelete("card={cardId}&user={userGuid}")]
        public async Task<ActionResult> DeleteUserFromTeam(int cardId, string userGuid)
        {
            UserCard userCard = await db.UserCards.FirstOrDefaultAsync(x => x.IdCard == cardId && x.GuidUser.Equals(userGuid));

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
