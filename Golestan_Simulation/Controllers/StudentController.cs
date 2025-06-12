using Microsoft.AspNetCore.Mvc;

namespace Golestan_Simulation.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
