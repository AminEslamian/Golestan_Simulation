using Golestan_Simulation.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Golestan_Simulation.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class SectionsManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SectionsManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        //this is actually the index!:
        public async Task<IActionResult> Showsections() // Shows students of the selected section
        {
            var instructorId = User.FindFirstValue("UserId");

            var sections = await _context.Sections
                .Where(s => s.Teachs.Any(t => t.InstructorId.ToString() == instructorId))
                .Select(s => new
                {
                    Id = s.Id,
                    CourseName = s.Course.Name,
                    Semester = s.Semester,
                    Year = s.Year,
                    ClassroomName = s.Classroom.Id + " - " + s.Classroom.Building + " - " + s.Classroom.RoomNumber,
                })
                .ToListAsync();

            return View(sections);
        }




    }
}