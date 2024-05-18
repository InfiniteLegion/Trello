using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Models;

namespace Trello.Controllers
{
    [Route("api/card-tags")]
    [ApiController]
    public class CardTagController : ControllerBase
    {
        private CheloDbContext db;

        public CardTagController(CheloDbContext db) { this.db = db; }

        [HttpPost]
        public async Task<ActionResult> AddTagToCard(CardTag cardTag)
        {
            CardTag fromDb = await db.CardTags.FirstOrDefaultAsync(x => x.IdCard == cardTag.IdCard && x.IdTags == cardTag.IdTags);

            if (fromDb != null)
            {
                return BadRequest("This CardTag object already exists");
            }

            if (cardTag == null)
            {
                return BadRequest("CardTag object is null");
            }

            await db.CardTags.AddAsync(cardTag);
            await db.SaveChangesAsync();
            return Ok("Tag added to card");
        }

        [HttpDelete("card={cardId}&tag={tagId}")]
        public async Task<ActionResult> RemoveTagFromCard(int cardId, int tagId)
        {
            CardTag cardTag = await db.CardTags.FirstOrDefaultAsync(x => x.IdCard == cardId && x.IdTags == tagId);

            if (cardTag == null)
            {
                return BadRequest("CardTag object is null");
            }

            db.CardTags.Remove(cardTag);
            await db.SaveChangesAsync();
            return Ok("Tag removed from card");
        }
    }
}
