using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Golestan_Simulation.Areas.Admin.Controllers
{
    public class CoursesManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CoursesManagementController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CreateCourse()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(CourseViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newCourse = new Courses
                {
                    Name = vm.Name,
                    Code = vm.Code,
                    Unit = vm.Unit,
                    Description = vm.Description,
                    ExameDate = vm.ExamDate,
                };
                await _context.Courses.AddAsync(newCourse);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }


        // Optional: for confirmation page(if confirmed set the view page)
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(s => s.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course); // A confirmation view
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(s => s.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
