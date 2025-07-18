﻿using Golestan_Simulation.Data;
using Golestan_Simulation.Services;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Golestan_Simulation.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class Dashboard : Controller
    {
        private readonly ApplicationDbContext _context;

        public Dashboard (ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ShowSections()
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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

            ViewBag.Sections = sections;
            return View();
        }

    }
}