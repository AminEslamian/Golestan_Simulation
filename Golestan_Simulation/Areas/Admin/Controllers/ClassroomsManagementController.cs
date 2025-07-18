using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Golestan_Simulation.Areas.Admin.Controllers
{
    public class ClassroomsManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ClassroomsManagementController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CreateClassroom()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClassroom(ClassroomViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newClass = new Classrooms
                {
                    Building = vm.Building,
                    RoomNumber = vm.RoomNumber,
                    Capacity = vm.Capacity
                };

                await _context.Classrooms.AddAsync(newClass);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }
    }
}
