using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Models;

namespace Trello.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly CheloDbContext db;

        public FriendshipController(CheloDbContext db)
        {
            this.db = db;
        }

        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<Friendship>> GetUserFriendships(int userId)
        {
            var friendships = db.Friendships
                .Where(f => f.IdUser1Sender == userId || f.IdUser2Receiver == userId)
                .ToList();

            return Ok(friendships);
        }

        [HttpPost]
        public ActionResult AddFriendship([FromBody] Friendship friendship)
        {
            if (FriendshipExists((int)friendship.IdUser1Sender, (int)friendship.IdUser2Receiver))
            {
                return Conflict(new { message = "Friendship already exists" });
            }

            db.Friendships.Add(friendship);
            db.SaveChanges();

            return CreatedAtAction(nameof(GetUserFriendships), new { userId = friendship.IdUser1Sender }, friendship);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateFriendshipStatus(int id, [FromBody] string status)
        {
            var friendship = db.Friendships.Find(id);
            if (friendship == null)
            {
                return NotFound();
            }

            friendship.Status = status;
            db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteFriendship(int id)
        {
            var friendship = db.Friendships.Find(id);
            if (friendship == null)
            {
                return NotFound();
            }

            db.Friendships.Remove(friendship);
            db.SaveChanges();

            return NoContent();
        }

        private bool FriendshipExists(int userId1, int userId2)
        {
            return db.Friendships
                .Any(f => (f.IdUser1Sender == userId1 && f.IdUser2Receiver == userId2) ||
                          (f.IdUser1Sender == userId2 && f.IdUser2Receiver == userId1));
        }
    }
}
