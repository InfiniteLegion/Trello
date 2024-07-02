using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Classes.Mapper
{
    public class ConfigurationMapper
    {
        private CheloDbContext db;

        public ConfigurationMapper(CheloDbContext db)
        {
            this.db = db;
        }

        public async Task<ConfigurationDTO> ToDTO(Configuration configuration)
        {
            ConfigurationDTO configurationDTO = new ConfigurationDTO()
            {
                Id = configuration.Id,
                GuidUser = configuration.GuidUser,
                IsprivateTeamNotification = configuration.IsprivateTeamNotifications
            };

            return configurationDTO;
        }
    }
}
