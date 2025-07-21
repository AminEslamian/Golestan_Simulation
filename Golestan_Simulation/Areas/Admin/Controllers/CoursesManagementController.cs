using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Golestan_Simulation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CoursesManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CoursesManagementController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .AsNoTracking()
                .ToListAsync();

            return View(courses);
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


        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            return View(course);   // renders Delete.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection form)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Name,Code,Unit,Description,ExamDate")] Courses vm)
        {
            // If user submitted invalid data, re-fetch the original so the view has Id & Capacity
            if (!ModelState.IsValid)
            {
                var original = await _context.Courses.FindAsync(id);
                if (original == null)
                    return NotFound();

                // overwrite the two editable fields so the form redisplays user input
                original.Name = vm.Name;
                original.Code = vm.Code;
                original.Unit = vm.Unit;
                original.Description = vm.Description;
                original.ExameDate = vm.ExameDate;
                return View(original);
            }

            // Now perform the real update
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            course.Name = vm.Name;
            course.Code = vm.Code;
            course.Unit = vm.Unit;
            course.Description = vm.Description;
            course.ExameDate = vm.ExameDate;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Info(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();
            return View(course);
        }

    }
}
