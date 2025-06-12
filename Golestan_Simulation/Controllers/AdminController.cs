using Microsoft.AspNetCore.Mvc;

namespace Golestan_Simulation.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
