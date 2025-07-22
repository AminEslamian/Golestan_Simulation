﻿using Golestan_Simulation.Data;
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

            var instructorId = int.Parse(User.FindFirstValue("DefaultInstructorId"));


            //var sections = await _context.Sections
            //    .Include(s => s.Teachs)
            //    .Where(s => s.Teachs.)
            //    .Select(s => new
            //    {
            //        Id = s.Id,
            //        CourseName = s.Course.Name,
            //        Semester = s.Semester,
            //        Year = s.Year,
            //        ClassroomName = s.Classroom.Id + " - " + s.Classroom.Building + " - " + s.Classroom.RoomNumber,
            //    })
            //    .ToListAsync();
            var sections = await _context.Teaches
                .Include(t => t.Section).ThenInclude(s => s.Course)
                .Include(t => t.Section).ThenInclude(s => s.Classroom)
                .Where(t => t.InstructorId == instructorId)
                .Select(t => new
                {
                    Id = t.Section.Id,
                    CourseName = t.Section.Course.Name,
                    Semester = t.Section.Semester,
                    Year = t.Section.Year,
                    ClassroomName = $"{t.Section.Classroom.Id} {t.Section.Classroom.Building} - {t.Section.Classroom.RoomNumber}"
                }).ToListAsync();

            ViewBag.Sections = sections;

            return View();
        }

        public async Task<IActionResult> SetGrade(int studentId, int sectionId)
        {
            var take = await _context.Takes
                .Include(t => t.Student).ThenInclude(s => s.User)
                .Include(t => t.Section).ThenInclude(s => s.Course)
                .FirstOrDefaultAsync(t =>
                    t.StudentId == studentId &&
                    t.SectionId == sectionId);

            if (take == null)
                return NotFound();

            return View(take);
        }

        [HttpPost, ActionName("SetGrade")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetGradeConfirmed(int studentId, int sectionId, byte grade)
        {
            var take = await _context.Takes
                .FirstOrDefaultAsync(t =>
                    t.StudentId == studentId &&
                    t.SectionId == sectionId);

            if (take == null)
                return NotFound();

            // update grade
            take.Grade = grade;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Showsections));
        }

        public async Task<IActionResult> Info(int sectionId)
        {
            var instructorId = int.Parse(User.FindFirstValue("DefaultInstructorId"));

            // verify this instructor actually teaches this section
            bool teaches = await _context.Teaches
                .AnyAsync(t => t.InstructorId == instructorId && t.SectionId == sectionId);
            if (!teaches)
                return Forbid();

            // load all takes for this section, including student & course info
            var takes = await _context.Takes
                .Include(t => t.Student).ThenInclude(s => s.User)
                .Include(t => t.Section).ThenInclude(s => s.Course)
                .Where(t => t.SectionId == sectionId)
                .ToListAsync();

            // pass section metadata for header
            ViewBag.Section = takes.FirstOrDefault()?.Section;

            return View(takes);
        }
    }
}