using Microsoft.AspNetCore.Mvc;

namespace Golestan_Simulation.Areas.Instructor.Controllers
{
    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
