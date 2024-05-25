using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Classes;
using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private CheloDbContext db;

        public UsersController(CheloDbContext db) {  this.db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetAllUsers()
        {
            return await db.UserInfos.ToListAsync();
        }

        // GET /users/5  5 - приклад id користувача
        [HttpGet("{guid}")]
        public async Task<ActionResult<UserInfo>> GetUserByGuid(string guid)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(guid));
            if (user == null)
            {
                return BadRequest("User not found");
            }
            return new ObjectResult(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserInfo>> AddUser(UserInfo user)
        {
            if (user == null)
            {
                return BadRequest("User is null");
            }

            string potentialError = UserValidator.IsUserAlreadyExists(db, user);
            if (potentialError != null)
            {
                return BadRequest(potentialError);
            }

            user.Guid = Guid.NewGuid().ToString();

            await db.UserInfos.AddAsync(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult<UserInfo>> UpdateUser(UserInfo user)
        {
            if (user == null)
            {
                return BadRequest("User is null");
            }

            if (!db.UserInfos.Any(x => x.Guid.Equals(user.Guid)))
            {
                return BadRequest("User not found");
            }

            UserInfo originalUser = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(user.Guid));
            
            UserValidator.CheckUserUpdate(user, originalUser);
            await db.SaveChangesAsync();
            return Ok(originalUser);
        }

        [HttpDelete("{guid}")]
        public async Task<ActionResult> DeleteUserByGuid(string guid)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(guid));

            if (user == null)
            {
                return BadRequest("User not found");
            }

            db.UserInfos.Remove(user);
            await db.SaveChangesAsync();
            return Ok("User deleted");
        }

        [HttpPost("auth")]
        public async Task<ActionResult<UserDto>> Auth(UserInfo user)
        {
            string? potentialError = UserValidator.CheckUserAuth(db, user);

            if (potentialError != null)
            {
                return BadRequest(potentialError);
            }
            else
            {
                UserInfo userInfo;

                if (user.Email == String.Empty || user.Email == "" || user.Email == null)
                {
                    userInfo = await db.UserInfos.FirstOrDefaultAsync(x => x.Username.Equals(user.Username));
                }
                else
                {
					userInfo = await db.UserInfos.FirstOrDefaultAsync(x => x.Email.Equals(user.Email));
				}

                UserDto userDto = new UserDto() { Email = userInfo.Email, UserName = userInfo.Username, Guid = userInfo.Guid };
                return Ok(userDto);
            }
        }
        [HttpGet("boards/{userId}")]
        public ActionResult<IEnumerable<Board>> GetUserBoards(int userId)
        {
            var userBoards = db.UserCards
                .Where(uc => uc.IdUser == userId)
                .Select(uc => uc.IdCard)
                .Distinct() 
                .ToList();

            var boards = db.Cards
                .Where(c => userBoards.Contains(c.Id))
                .Select(c => c.IdBoard)
                .Distinct() 
                .ToList();

            var userBoardDetails = db.Boards
                .Where(b => boards.Contains(b.Id))
                .ToList();

            return userBoardDetails;
        }
    }
}
