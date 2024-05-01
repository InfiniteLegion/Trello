using Microsoft.EntityFrameworkCore;
using Trello.Models;

namespace Trello.Classes
{
    public class UserValidator
    {
        public static void CheckUserUpdate(UserInfo userToUpdate, UserInfo originalUser)
        {
            if (userToUpdate.Username != null)
            {
                originalUser.Username = userToUpdate.Username;
            }
            if (userToUpdate.Password != null)
            {
                originalUser.Password = userToUpdate.Password;
            }
            if (userToUpdate.Email != null)
            {
                originalUser.Email = userToUpdate.Email;
            }
        }

        public static string? IsUserAlreadyExists(CheloDbContext db, UserInfo newUser)
        {
            bool email = db.UserInfos.FirstOrDefault(x => x.Email.Equals(newUser.Email)) == null ? false : true;
            bool userName = db.UserInfos.FirstOrDefault(x => x.Username.Equals(newUser.Username)) == null ? false : true;

            if (email && userName)
            {
                return "User with this email and username already exists";
            }
            else if (email)
            {
                return "User with this email already exists";
            }
            else if (userName)
            {
                return "User with this username already exists";
            }
            else
            {
                return null;
            }
        }

        public static string? CheckUserAuth(CheloDbContext db, UserInfo user)
        {
            UserInfo? optionalUser = db.UserInfos.FirstOrDefault(x => x.Email.Equals(user.Email));

            if (optionalUser == null)
            {
                return "Wrong email or user doesn't exists";
            }
            else
            {
                if (!optionalUser.Password.Equals(user.Password))
                {
                    return "Wrong password";
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
