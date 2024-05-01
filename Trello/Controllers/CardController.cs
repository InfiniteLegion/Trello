using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Classes;
using Trello.Models;

namespace Trello.Controllers
{
    [ApiController]
    [Route("api/cards")]
    public class CardController : Controller
    {
        private CheloDbContext db;

        public CardController(CheloDbContext db) { this.db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetAllCards()
        {
            return await db.Cards.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCardById(int id)
        {
            Card card = await db.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (card == null)
            {
                return BadRequest("Card not found");
            }
            return new ObjectResult(card);
        }

        [HttpPost]
        public async Task<ActionResult<Card>> AddCard(Card card)
        {
            if (card == null)
            {
                return BadRequest("Card is null");
            }

            await db.Cards.AddAsync(card);
            await db.SaveChangesAsync();
            return Ok(card);
        }

        [HttpPut]
        public async Task<ActionResult<Card>> UpdateCard(Card card)
        {
            if (card == null)
            {
                return BadRequest("Card is null");
            }
            if (!db.Cards.Any(x => x.Id == card.Id))
            {
                return BadRequest("Card not found");
            }

            Card originalCard = await db.Cards.FirstOrDefaultAsync(x => x.Id == card.Id);

            CardValidator.CheckCardUpdate(card, originalCard);
            await db.SaveChangesAsync();
            return Ok(card);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCardById(int id)
        {
            Card card = await db.Cards.FirstOrDefaultAsync(x => x.Id == id);

            if (card == null)
            {
                return BadRequest("Card not found");
            }

            db.Cards.Remove(card);
            await db.SaveChangesAsync();
            return Ok("Card deleted");
        }

        [HttpGet("{cardId}/tasks")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllCardTasks(int cardId)
        {
            Card card = await db.Cards.FirstOrDefaultAsync(x => x.Id == cardId);

            if (card == null)
            {
                return BadRequest("Card not found");
            }

            var tasks = await db.Tasks.Where(x=>x.IdCard == cardId).ToListAsync();

            return tasks;
        }
    }
}
