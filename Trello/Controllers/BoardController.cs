using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Classes.DTO;
using Trello.Classes.Mapper;
using Trello.Classes.Validator;
using Trello.Models;

namespace Trello.Controllers
{
    [Route("api/boards")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private CheloDbContext db;
        private readonly BoardMapper boardMapper;

        public BoardController(CheloDbContext db, BoardMapper boardMapper) 
        {
            this.db = db; 
            this.boardMapper = boardMapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDTO>> GetBoardById(int id)
        {
            Board board = await db.Boards.FirstOrDefaultAsync(x => x.Id == id);
            if (board == null)
            {
                return BadRequest("Board not found");
            }

            BoardDTO boardDTO = await boardMapper.ToDTO(board);

            return new ObjectResult(boardDTO);
        }

        [HttpPost]
        public async Task<ActionResult<Board>> AddBoard(Board board)
        {
            if (board == null)
            {
                return BadRequest("Board is null");
            }

            await db.Boards.AddAsync(board);
            await db.SaveChangesAsync();
            return Ok(board);
        }

        [HttpPut]
        public async Task<ActionResult<Board>> UpdateBoard(Board board)
        {
            if (board == null)
            {
                return BadRequest("Board is null");
            }
            if (!db.Boards.Any(x => x.Id == board.Id))
            {
                return BadRequest("Board not found");
            }

            Board originalBoard = await db.Boards.FirstOrDefaultAsync(x => x.Id == board.Id);

            BoardValidator.CheckBoardUpdate(board, originalBoard);
            await db.SaveChangesAsync();
            return Ok(board);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBoardById(int id)
        {
            Board board = await db.Boards.FirstOrDefaultAsync(x => x.Id == id);

            if (board == null)
            {
                return BadRequest("Board not found");
            }

            db.Boards.Remove(board);
            await db.SaveChangesAsync();
            return Ok("Board deleted");
        }

        [HttpGet("{boardId}/cards")]
        public async Task<ActionResult<IEnumerable<Card>>> GetAllBoardCards(int boardId)
        {
            Board board = await db.Boards.FirstOrDefaultAsync(x => x.Id == boardId);

            if (board == null)
            {
                return BadRequest("Board not found");
            }

            var cards = await db.Cards.Where(x => x.IdBoard == boardId).ToListAsync();

            return cards;
        }

        [HttpGet("{boardId}/tags")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetAllBoardTags(int boardId)
        {
            Board board = await db.Boards.FirstOrDefaultAsync(x => x.Id == boardId);

            if (board == null)
            {
                return BadRequest("Board not found");
            }

            var tags = await db.Tags.Where(x => x.IdBoard == boardId).ToListAsync();

            return tags;
        }
        [HttpGet("{boardId}/status-columns")]
        public async Task<ActionResult<IEnumerable<StatusColumn>>> GetStatusColumns(int boardId)
        {
            Board board = await db.Boards.FirstOrDefaultAsync(x => x.Id == boardId);

            if (board == null)
            {
                return BadRequest("Board not found");
            }

            var statusColumns = await db.StatusColumns.Where(x => x.IdBoard == board.Id).ToListAsync();

            return statusColumns;
        }
    }
}
