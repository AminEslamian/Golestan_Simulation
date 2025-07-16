using Microsoft.AspNetCore.Mvc;

namespace Golestan_Simulation.Areas.Student.Controllers
{
    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
