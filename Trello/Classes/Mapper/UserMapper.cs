using Microsoft.EntityFrameworkCore;
using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Classes.Mapper
{
    public class UserMapper
    {
        private CheloDbContext db;
        private readonly ConfigurationMapper configurationMapper;

        public UserMapper(CheloDbContext db, ConfigurationMapper configurationMapper) {
            this.db = db;
            this.configurationMapper = configurationMapper;
        }

        public async Task<UserDTO> ToDTO(UserInfo user)
        {
            if (user == null)
            {
                return null;
            }

            UserDTO userDTO = new UserDTO()
            {
                UserName = user.Username,
                Email = user.Email,
                Guid = user.Guid
            };

            Configuration configuration = await db.Configurations.FirstOrDefaultAsync(x => x.GuidUser.Equals(user.Guid));
            ConfigurationDTO configurationDTO = await configurationMapper.ToDTO(configuration);

            userDTO.ConfigurationDTO = configurationDTO;

            return userDTO;
        }
    }
}
