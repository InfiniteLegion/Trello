using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Classes;
using Trello.Classes.DTO;
using Trello.Classes.Mapper;
using Trello.Models;

namespace Trello.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private CheloDbContext db;
        private readonly UserMapper mapper;

        public UsersController(CheloDbContext db, UserMapper mapper) 
        {  
            this.db = db; 
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetAllUsers()
        {
            return await db.UserInfos.ToListAsync();
        }

        // GET /users/5  5 - приклад id користувача
        [HttpGet("{guid}")]
        public async Task<ActionResult<UserDto>> GetUserByGuid(string guid)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Guid.Equals(guid));
            if (user == null)
            {
                return BadRequest("User not found");
            }

            UserDto userDto = await mapper.ToDTO(user);
            return new ObjectResult(userDto);
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
        [HttpGet("boards/{userGuid}")]
        public async Task<ActionResult<IEnumerable<Board>>> GetUserBoards(string userGuid)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x=>x.Guid.Equals(userGuid));

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var teamUser = await db.TeamUsers.Where(x => x.IdUser == user.Id).ToListAsync();
            var teams = new List<Team>();
            foreach (var item in teamUser)
            {
                teams.Add(await db.Teams.FirstOrDefaultAsync(x => x.Id == item.IdTeam));
            }

            var boards = new List<Board>();
            foreach (var item in teams)
            {
                boards.Add(await db.Boards.FirstOrDefaultAsync(x => x.IdTeam == item.Id));
            }

            return boards;
        }
    }
}
