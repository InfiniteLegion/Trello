using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Classes.DTO;
using Trello.Classes.Mapper;
using Trello.Models;

namespace Trello.Controllers
{
    [Route("api/friendship")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly CheloDbContext db;
        private readonly FriendMapper mapper;

        public FriendshipController(CheloDbContext db, FriendMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        [HttpGet("friends/user={userGuid}")]
        public async Task<ActionResult<IEnumerable<FriendDTO>>> GetUserFriendships(string userGuid)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(userGuid));
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var friendships = await db.Friendships
                .Where(f => (f.IdUser1Sender == user.Id || f.IdUser2Receiver == user.Id) && f.Status == "ACCEPTED")
                .ToListAsync();

            var friends = new List<UserInfo>();
            foreach (var f in friendships)
            {
                long friendId = (long)(f.IdUser1Sender != user.Id ? f.IdUser1Sender : f.IdUser2Receiver);
                UserInfo friend = await db.UserInfos
                    .FirstOrDefaultAsync(x => x.Id == friendId);
                friends.Add(friend);
            }

            var friendDTOs = new List<FriendDTO>();
            foreach (var f in friends)
            {
                var friendDTO = await mapper.ToDTO(f);
                friendDTOs.Add(friendDTO);
            }

            return Ok(friendDTOs);
        }

        [HttpGet("requests/user={userGuid}")]
        public async Task<ActionResult<IEnumerable<FriendDTO>>> GetUserFriendshipRequests(string userGuid)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(userGuid));
            if (user == null)
            {
                return BadRequest("Wrong guid");
            }

            var friendships = await db.Friendships
                .Where(f => (f.IdUser1Sender == user.Id || f.IdUser2Receiver == user.Id) && f.Status == "WAITING")
                .ToListAsync();

            var requests = new List<UserInfo>();
            foreach (var f in friendships)
            {
                long senderId = (long)(f.IdUser1Sender != user.Id ? f.IdUser1Sender : f.IdUser2Receiver);
                UserInfo sender = await db.UserInfos
                    .FirstOrDefaultAsync(x => x.Id == senderId);
                requests.Add(sender);
            }

            var requestDTOs = new List<FriendDTO>();
            foreach (var f in requests)
            {
                var senderDTO = await mapper.ToDTO(f);
                requestDTOs.Add(senderDTO);
            }

            return Ok(requestDTOs);
        }

        [HttpPost("add/sender={guidSender}&receiver={guidReceiver}")]
        public async Task<ActionResult<Friendship>> AddFriendship(string guidSender, string guidReceiver)
        {
            UserInfo sender = await db.UserInfos
                .FirstOrDefaultAsync(x => x.Guid.Equals(guidSender));
            UserInfo receiver = await db.UserInfos
                .FirstOrDefaultAsync(x => x.Guid.Equals(guidReceiver));
            if (sender == null || receiver == null)
            {
                return BadRequest("Users not found");
            }

            if (sender.Id == receiver.Id)
            {
                return BadRequest("You cannot add yourself to your friendlist.");
            }

            if (FriendshipExists((int)sender.Id, (int)receiver.Id))
            {
                return BadRequest("Friendship already exists");
            }

            Friendship friendship = new Friendship() { IdUser1Sender = sender.Id, IdUser2Receiver = receiver.Id, Status = "WAITING" };
            db.Friendships.Add(friendship);
            db.SaveChanges();

            return Ok(friendship);
        }

        [HttpPut("sender={guid}&decision={decision}")]
        public async Task<ActionResult> AcceptDenyFriendship(string guid, string decision)
        {
            UserInfo sender = await db.UserInfos
                .FirstOrDefaultAsync(x => x.Guid.Equals(guid));
            Friendship friendship = await db.Friendships
                .FirstOrDefaultAsync(x => x.IdUser1Sender == sender.Id && x.Status.Equals("WAITING"));

            if (friendship == null)
            {
                return BadRequest("Wrong id");
            }

            if (decision.ToLower().Equals("accept"))
            {
                friendship.Status = "ACCEPTED";
                await db.SaveChangesAsync();
                return Ok("Friendship accepted");
            }
            else if (decision.ToLower().Equals("deny"))
            {
                db.Friendships.Remove(friendship);
                await db.SaveChangesAsync();
                return Ok("Friendship denied");
            }
            else
            {
                return Ok("Wrong command");
            }
        }

        [HttpDelete("delete/user={userGuid}&friend={friendGuid}")]
        public async Task<ActionResult> DeleteFriendship(string userGuid, string friendGuid)
        {
            UserInfo user = await db.UserInfos
                .FirstOrDefaultAsync(x => x.Guid.Equals(userGuid));
            UserInfo friend = await db.UserInfos
                .FirstOrDefaultAsync(x=>x.Guid.Equals(friendGuid));

            var friendship = await db.Friendships
                .FirstOrDefaultAsync(f => (f.IdUser1Sender == user.Id && f.IdUser2Receiver == friend.Id)
                || (f.IdUser1Sender == friend.Id && f.IdUser2Receiver == user.Id));

            if (friendship == null)
            {
                return BadRequest("Friendship not found");
            }

            db.Friendships.Remove(friendship);
            db.SaveChanges();

            return Ok("Friendship deleted");
        }

        private bool FriendshipExists(int userId1, int userId2)
        {
            return db.Friendships
                .Any(f => (f.IdUser1Sender == userId1 && f.IdUser2Receiver == userId2) ||
                          (f.IdUser1Sender == userId2 && f.IdUser2Receiver == userId1));
        }
    }
}
