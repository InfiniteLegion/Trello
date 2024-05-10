using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Models;

namespace Trello.Controllers
{
    [Route("api/boardtags")]
    [ApiController]
    public class BoardTagController : ControllerBase
    {
        private CheloDbContext db;

        public BoardTagController(CheloDbContext db) { this.db = db; }

        
        [HttpPost]
        public async Task<ActionResult> CreateTag(BoardTag boardTag)
        {
            if (boardTag == null)
            {
                return BadRequest("BoardTag object is null");
            }

            await db.BoardTags.AddAsync(boardTag);
            await db.SaveChangesAsync();
            return Ok("Tag added to board");
        }

        [HttpDelete("tag={tagId}&board={boardId}")]
        public async Task<ActionResult> DeleteTagFromBoard(int tagId, int boardId)
        {
            BoardTag boardTag = await db.BoardTags.FirstOrDefaultAsync(x => x.IdBoard == boardId && x.IdTags == tagId);

            if (boardTag == null)
            {
                return BadRequest("BoardTag object not found");
            }

            db.BoardTags.Remove(boardTag);
            await db.SaveChangesAsync();
            return Ok("Tag deleted from board");
        }
        [HttpGet("{boardId}/tags")]
        public async Task<ActionResult<IEnumerable<BoardTag>>> GetAllBoardCards(int boardId)
        {
            Board board = await db.Boards.FirstOrDefaultAsync(x => x.Id == boardId);

            if (board == null)
            {
                return BadRequest("Board not found");
            }

            var tags = await db.BoardTags.Where(x => x.IdBoard == boardId).ToListAsync();

            return tags;
        }

    }
}
