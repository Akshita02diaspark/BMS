
using Microsoft.AspNetCore.Mvc;


namespace BMS.Areas.Users.Controllers
{
    [Area("Users")]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
         return View();
        }
    }
}
