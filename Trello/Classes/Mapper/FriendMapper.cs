using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Classes.DTO;
using Trello.Models;

namespace Trello.Classes.Mapper
{
    public class FriendMapper
    {
        private readonly CheloDbContext db;

        public FriendMapper(CheloDbContext db)
        {
            this.db = db;
        }

        public async Task<FriendDTO> ToDTO(UserInfo friend)
        {
            return new FriendDTO { Guid = friend.Guid, Name = friend.Username };
        }
    }
}
