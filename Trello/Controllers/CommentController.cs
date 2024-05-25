using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Classes.Validator;
using Trello.Models;

namespace Trello.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CheloDbContext db;

        public CommentController(CheloDbContext db)
        {
            this.db = db;
        }

        [HttpPost]
        public async Task<ActionResult<CardComment>> AddComment(CardComment comment)
        {
            if (comment == null)
            {
                return BadRequest("Comment is null");
            }

            if (comment.CommentDatetime == null)
            {
                comment.CommentDatetime = DateTime.Now;
            }

            await db.CardComments.AddAsync(comment);
            await db.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpPut]
        public async Task<ActionResult<CardComment>> UpdateComment(CardComment comment)
        {
            if (comment == null)
            {
                return BadRequest("Comment is null");
            }
            if (!db.CardComments.Any(x=>x.Id==comment.Id))
            {
                return BadRequest("Comment not found");
            }

            CardComment originalComment = await db.CardComments.FirstOrDefaultAsync(x => x.Id == comment.Id);

            CommentValidatior.CheckCommentUpdate(comment, originalComment);
            await db.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCommentById(int id)
        {
            CardComment comment = await db.CardComments.FirstOrDefaultAsync(x => x.Id == id);

            if (comment == null)
            {
                return BadRequest("Comment not found");
            }

            db.CardComments.Remove(comment);
            await db.SaveChangesAsync();
            return Ok("Comment removed");
        }
    }
}
