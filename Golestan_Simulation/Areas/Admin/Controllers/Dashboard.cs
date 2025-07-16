using Microsoft.AspNetCore.Mvc;

namespace Golestan_Simulation.Areas.Admin.Controllers
{
    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
