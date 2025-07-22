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
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var students = await _context.Students
                .Include(st => st.User)  // ← Include on the root entity
                .Where(st => st.Takes
                    .Any(t => t.Section.Teachs
                        .Any(te => te.InstructorId.ToString() == instructorId)))
                .ToListAsync();

            return View(students);
        }

    }
}
