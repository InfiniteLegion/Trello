using Microsoft.AspNetCore.Mvc;

namespace Trello.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
