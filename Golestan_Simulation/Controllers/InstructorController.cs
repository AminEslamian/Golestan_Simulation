using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Golestan_Simulation.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class InstructorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
