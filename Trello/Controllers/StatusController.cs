using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Trello.Classes;
using Trello.Models;

namespace Trello.Controllers
{
    [Route("api/statuses")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private CheloDbContext db;

        public StatusController(CheloDbContext db)
        {
            this.db = db;
        }

        // GET: api/status
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusColumn>>> GetAllStatuses()
        {
            return await db.StatusColumns.ToListAsync();
        }

        // GET: api/status/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<StatusColumn>> GetStatusById(int id)
        {
            StatusColumn status = await db.StatusColumns.FirstOrDefaultAsync(x => x.Id == id);
            if (status == null)
            {
                return NotFound();
            }
            return new ObjectResult(status);
        }

        // POST: api/status
        [HttpPost]
        public async Task<ActionResult<StatusColumn>> AddStatus(StatusColumn status)
        {
            if (status == null)
            {
                return BadRequest("Status is null");
            }

            await db.StatusColumns.AddAsync(status);
            await db.SaveChangesAsync();
            return Ok(status);
        }

        // PUT: api/status/
        [HttpPut]
        public async Task<ActionResult<StatusColumn>> UpdateStatus(StatusColumn status)
        {
            if (status == null)
            {
                return BadRequest("Status is null");
            }
            if (!db.StatusColumns.Any(x => x.Id == status.Id))
            {
                return BadRequest("Status not found");
            }

            StatusColumn originalStatus = await db.StatusColumns.FirstOrDefaultAsync(x => x.Id == status.Id);

            StatusValidator.CheckStatusUpdate(status, originalStatus);
            await db.SaveChangesAsync();
            return Ok(originalStatus);
        }

        // DELETE: api/status/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStatusById(int id)
        {
            StatusColumn status = await db.StatusColumns.FirstOrDefaultAsync(x => x.Id == id);

            if (status == null)
            {
                return BadRequest("Status not found");
            }

            db.StatusColumns.Remove(status);
            await db.SaveChangesAsync();
            return Ok("Status deleted");
        }
    }
}
