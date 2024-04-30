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
    }
}
