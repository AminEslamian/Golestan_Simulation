using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.Services;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Golestan_Simulation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ClassroomsManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICreateClassroomService _classroomservice;
        public ClassroomsManagementController(ApplicationDbContext context, ICreateClassroomService classroomservice)
        {
            _context = context;
            _classroomservice = classroomservice;
        }


        public async Task<IActionResult> Index()
        {
            var classrooms = await _context.Classrooms
                .AsNoTracking()
                .ToListAsync();

            return View(classrooms);
        }


        public IActionResult CreateClassroom()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClassroom(ClassroomViewModel vm)
        {
            if (await _classroomservice.IsClassroomAvailableAsync(vm.Building, vm.RoomNumber))
            {
                ModelState.AddModelError("Building", "This class is not available");
                return View(vm);
            }
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


        //  GET: show confirmation
        //    URL: /Admin/ClassroomsManagement/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null) return NotFound();
            return View(classroom);   // renders Delete.cshtml
        }

        // 2) POST: perform deletion
        //    Form posts back to the *same* /Delete/{id} URL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection form)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom != null)
            {
                _context.Classrooms.Remove(classroom);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
                return NotFound();

            return View(classroom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Building,RoomNumber,Capacity")] Classrooms vm)
        {
            // If user submitted invalid data, re-fetch the original so the view has Id & Capacity
            if (!ModelState.IsValid)
            {
                var original = await _context.Classrooms.FindAsync(id);
                if (original == null)
                    return NotFound();

                // overwrite the two editable fields so the form redisplays user input
                original.Building = vm.Building;
                original.RoomNumber = vm.RoomNumber;
                return View(original);
            }

            // Now perform the real update
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
                return NotFound();

            classroom.Building = vm.Building;
            classroom.RoomNumber = vm.RoomNumber;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
