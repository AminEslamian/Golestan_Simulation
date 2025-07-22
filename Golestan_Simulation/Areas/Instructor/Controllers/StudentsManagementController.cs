using Golestan_Simulation.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Golestan_Simulation.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class StudentsManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentsManagementController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ShowStudents()
        {
            var instructorId = int.Parse(User.FindFirstValue("DefaultInstructorId"));

            var takes = await _context.Takes
                .Include(t => t.Student).ThenInclude(s => s.User)
                .Include(t => t.Section).ThenInclude(s => s.Course)
                .Where(t => t.Section.Teachs
                    .Any(te => te.InstructorId == instructorId))
                .ToListAsync();

            return View(takes);
        }


        // GET: Instructor/StudentsManagement/DeleteTake?studentId=123§ionId=456
        public async Task<IActionResult> DeleteTake(int studentId, int sectionId)
        {
            var t = await _context.Takes
                .Include(x => x.Student).ThenInclude(s => s.User)
                .Include(x => x.Section).ThenInclude(s => s.Course)
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.SectionId == sectionId);
            if (t == null) return NotFound();
            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTake(int studentId, int sectionId, IFormCollection form)
        {
            var t = await _context.Takes.FindAsync(studentId, sectionId);
            if (t != null)
            {
                _context.Takes.Remove(t);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ShowStudents));
        }


    }
}
