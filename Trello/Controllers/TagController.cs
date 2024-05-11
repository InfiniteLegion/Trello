using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Trello.Models;
using Trello.Classes;
using Trello.Classes.Validator;

namespace Trello.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private CheloDbContext db;

        public TagController(CheloDbContext db) { this.db = db; }
        [HttpPut]
        public async Task<ActionResult<Tag>> UpdateTag(Tag tag)
        {
            if (tag == null)
            {
                return BadRequest("Tag is null");
            }
            if(!db.Tags.Any(x => x.Id == tag.Id))
            {
                return BadRequest("Tag is not found");
            }
            Tag originalTag = await db.Tags.FirstOrDefaultAsync(x => x.Id == tag.Id);
            TagValidator.CheckTagUpdate(tag, originalTag);
            await db.SaveChangesAsync();
            return Ok(tag);
        }
        
        //[HttpPost]
        //public async Task<ActionResult<Tag>> CreateTag(Tag tag)
        //{

        //    if (tag == null)
        //    {
        //        return BadRequest("Tag is null");
        //    }

        //    await db.Tags.AddAsync(tag);
        //    await db.SaveChangesAsync();
        //    return Ok(tag);
        //}
    }
}
