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


        public async Task<IActionResult> DeleteTake(int studentId, int sectionId)
        {
            var take = await _context.Takes
                .Include(t => t.Student).ThenInclude(s => s.User)
                .Include(t => t.Section).ThenInclude(s => s.Course)
                .FirstOrDefaultAsync(t => t.StudentId == studentId && t.SectionId == sectionId);

            if (take == null)
                return NotFound();

            return View(take);
        }

        [HttpPost, ActionName("DeleteTake")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTakeConfirmed(int studentId, int sectionId)
        {
            var take = await _context.Takes
                .FirstOrDefaultAsync(t => t.StudentId == studentId && t.SectionId == sectionId);

            if (take == null)
                return NotFound();

            _context.Takes.Remove(take);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ShowStudents));
        }


    }
}
