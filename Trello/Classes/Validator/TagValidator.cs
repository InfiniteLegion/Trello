using Trello.Models;

namespace Trello.Classes.Validator
{
    public class TagValidator
    {
        public static void CheckTagUpdate(Tag tagToUpdate, Tag originalTag)
        {
            if (tagToUpdate.Name != null)
            {
                originalTag.Name = tagToUpdate.Name;
            }
        }
    }
}
