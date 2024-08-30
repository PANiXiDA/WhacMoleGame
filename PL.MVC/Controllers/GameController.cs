using Microsoft.AspNetCore.Mvc;

namespace PL.MVC.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
