using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Trello.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private CheloDbTestContext db;

        public UsersController(CheloDbTestContext db) {  this.db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> Get()
        {
            return await db.UserInfos.ToListAsync();
        }

        // GET /users/5  5 - приклад id користувача
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> Get(int id)
        {
            UserInfo user = await db.UserInfos.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }
    }
}
