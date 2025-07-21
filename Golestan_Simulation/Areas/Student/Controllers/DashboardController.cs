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

        // Optional: for confirmation page(if confirmed set the view page)
        public async Task<IActionResult> DeleteTake(int id) // the id is id of the selected takes which is intended to be deleted
        {
            var studentIdClaim = User.FindFirstValue("DefaultStudentId");
            if (string.IsNullOrEmpty(studentIdClaim) || !int.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            // load just that one take, belonging to this student
            var take = await _context.Takes
                .Include(t => t.Section)
                .FirstOrDefaultAsync(t => t.StudentId == id && t.StudentId == studentId);

            if (take == null) return NotFound();

            // pass it to the confirmation page
            return View(take);
        }


        [HttpPost, ActionName("DeleteTake")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTakeConfirmed(int id)
        {
            var studentIdClaim = User.FindFirstValue("DefaultStudentId");
            if (string.IsNullOrEmpty(studentIdClaim) || !int.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            //var classroom = await _context.Classrooms
            //    .FirstOrDefaultAsync(s => s.Id == id);
            var take = await _context.Takes
                .FirstOrDefaultAsync(t => t.StudentId == id && t.StudentId == studentId);

            if (take == null)
                return NotFound();


            _context.Takes.Remove(take);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}