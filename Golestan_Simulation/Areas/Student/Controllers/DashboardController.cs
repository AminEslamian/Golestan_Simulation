using Golestan_Simulation.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Golestan_Simulation.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1) pull the student id out of the auth cookie
            var studentIdClaim = User.FindFirstValue("DefaultStudentId");
            if (string.IsNullOrEmpty(studentIdClaim)
                || !int.TryParse(studentIdClaim, out var studentId))
            {
                return Forbid();  // or RedirectToAction("Login", "Home");
            }

            // 2) load only this student's enrollments, with Section → Course
            var takes = await _context.Takes
                .Where(t => t.StudentId == studentId)
                .Include(t => t.Section)
                    .ThenInclude(s => s.Course)
                .ToListAsync();

            // 3) hand them off to the view
            return View(takes);
        }

        // GET: Student/Dashboard/DeleteTake?sectionId=5
        [HttpGet]
        public async Task<IActionResult> DeleteTake(int sectionId)
        {
            var studentIdClaim = User.FindFirstValue("DefaultStudentId");
            if (string.IsNullOrEmpty(studentIdClaim)
                || !int.TryParse(studentIdClaim, out var studentId))
            {
                return Forbid();
            }

            // load the specific take by SectionId+StudentId
            var take = await _context.Takes
                .Include(t => t.Section)
                    .ThenInclude(s => s.Course)
                .FirstOrDefaultAsync(t => t.SectionId == sectionId
                                       && t.StudentId == studentId);

            if (take == null)
                return NotFound();

            return View(take);
        }

        // POST: Student/Dashboard/DeleteTake?sectionId=5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTake(int sectionId, IFormCollection form)
        {
            var studentIdClaim = User.FindFirstValue("DefaultStudentId");
            if (string.IsNullOrEmpty(studentIdClaim)
                || !int.TryParse(studentIdClaim, out var studentId))
            {
                return Forbid();
            }

            var take = await _context.Takes
                .FirstOrDefaultAsync(t => t.SectionId == sectionId
                                       && t.StudentId == studentId);

            if (take != null)
            {
                _context.Takes.Remove(take);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Scores()
        {
            var studentIdClaim = User.FindFirstValue("DefaultStudentId");
            if (string.IsNullOrEmpty(studentIdClaim) || !int.TryParse(studentIdClaim, out var studentId))
            {
                return Forbid();
            }

            // Load the student's courses and grades
            var takes = await _context.Takes
                .Where(t => t.StudentId == studentId)
                .Include(t => t.Section)
                    .ThenInclude(s => s.Course)
                .ToListAsync();

            // Pass directly to the view (we'll calculate average in the view)
            return View(takes);
        }

    }
}