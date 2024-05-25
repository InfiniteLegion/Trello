using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Classes.Mapper
{
    public class UserMapper
    {
        private CheloDbContext db;

        public UserMapper(CheloDbContext db) { this.db = db; }

        public async Task<UserDto> ToDTO(UserInfo user)
        {
            return new UserDto()
            {
                UserName = user.Username,
                Email = user.Email,
                Guid = user.Guid
            };
        }
    }
}
