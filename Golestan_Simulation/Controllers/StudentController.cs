using Golestan_Simulation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Golestan_Simulation.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
