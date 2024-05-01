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
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> GetUserById(int id)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Id == id);
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

            if (!db.UserInfos.Any(x => x.Id == user.Id))
            {
                return BadRequest("User not found");
            }

            UserInfo originalUser = await db.UserInfos.FirstOrDefaultAsync(x => x.Id == user.Id);
            
            UserValidator.CheckUserUpdate(user, originalUser);
            await db.SaveChangesAsync();
            return Ok(originalUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserById(int id)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            db.UserInfos.Remove(user);
            await db.SaveChangesAsync();
            return Ok("User deleted");
        }

        [HttpGet("auth")]
        public async Task<ActionResult<UserDto>> Auth(UserInfo user)
        {
            string? potentialError = UserValidator.CheckUserAuth(db, user);

            if (potentialError != null)
            {
                return BadRequest(potentialError);
            }
            else
            {
                UserInfo userInfo = await db.UserInfos.FirstOrDefaultAsync(x => x.Email.Equals(user.Email));
                UserDto userDto = new UserDto() { Id = userInfo.Id, Email = userInfo.Email, UserName = userInfo.Username, Role = userInfo.Role };
                return Ok(userDto);
            }
        }
    }
}
