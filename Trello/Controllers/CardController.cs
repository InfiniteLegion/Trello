using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Classes;
using Trello.Classes.DTO;
using Trello.Classes.Mapper;
using Trello.Models;

namespace Trello.Controllers
{
    [ApiController]
    [Route("api/cards")]
    public class CardController : Controller
    {
        private CheloDbContext db;
        private readonly CardMapper cardMapper;

        public CardController(CheloDbContext db, CardMapper cardMapper) 
        { 
            this.db = db;
            this.cardMapper = cardMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardDTO>>> GetAllCards()
        {
            var cards = await db.Cards.ToListAsync();
            var cardDTOs = new List<CardDTO>();

            foreach (var card in cards)
            {
                var cardDTO = await cardMapper.ToDTO(card);
                cardDTOs.Add(cardDTO);
            }

            return cardDTOs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CardDTO>> GetCardById(int id)
        {
            Card card = await db.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (card == null)
            {
                return BadRequest("Card not found");
            }

            CardDTO cardDTO = await cardMapper.ToDTO(card);
            return new ObjectResult(cardDTO);
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

            var tasks = await db.Tasks.Where(x => x.IdCard == card.Id).ToListAsync();
            if (tasks.Any())
            {
                foreach (var task in tasks)
                {
                    db.Tasks.Remove(task);
                }
            }

            var comments = await db.CardComments.Where(x => x.IdCard == card.Id).ToListAsync();
            if (comments.Any())
            {
                foreach (var comment in comments)
                {
                    db.CardComments.Remove(comment);
                }
            }

            var users = await db.UserCards.Where(x => x.IdCard == card.Id).ToListAsync();
            if (users.Any())
            {
                foreach (var user in users)
                {
                    db.UserCards.Remove(user);
                }
            }

            var tags = await db.CardTags.Where(x => x.IdCard == card.Id).ToListAsync();
            if (tags.Any())
            {
                foreach (var tag in tags)
                {
                    db.CardTags.Remove(tag);
                }
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

            var tasks = await db.Tasks.Where(x => x.IdCard == cardId).ToListAsync();

            return tasks;
        }

        [HttpGet("{cardId}/tags")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllCardTags(int cardId)
        {
            Card card = await db.Cards.FirstOrDefaultAsync(x => x.Id == cardId);

            if (card == null)
            {
                return BadRequest("Card not found");
            }

            var cardTags = await db.CardTags.Where(x => x.IdCard == cardId).ToListAsync();
            var tags = new List<Tag>();
            foreach (var item in cardTags)
            {
                tags.Add(await db.Tags.FirstOrDefaultAsync(x => x.Id == item.IdTags));
            }

            return tags;
        }

        [HttpGet("{cardId}/comments")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllCardComments(int cardId)
        {
            Card card = await db.Cards.FirstOrDefaultAsync(x => x.Id == cardId);

            if (card == null)
            {
                return BadRequest("Card not found");
            }

            var comments = await db.CardComments.Where(x => x.IdCard == cardId).ToListAsync();

            return comments;
        }
    }
}
