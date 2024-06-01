using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Classes.Mapper
{
    public class UserMapper
    {
        private CheloDbContext db;

        public UserMapper(CheloDbContext db) { this.db = db; }

        public async Task<UserDTO> ToDTO(UserInfo user)
        {
            return new UserDTO()
            {
                UserName = user.Username,
                Email = user.Email,
                Guid = user.Guid
            };
        }
    }
}
