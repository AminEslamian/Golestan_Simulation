using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.Services;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Golestan_Simulation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SectionsManagement : Controller
    {
        private readonly ApplicationDbContext _context;
        public SectionsManagement(ApplicationDbContext context)
        {
            _context = context;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}


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


        public IActionResult CreateClassroom()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClassroom(ClassroomViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newClass = new Classrooms
                {
                    Building = vm.Building,
                    RoomNumber = vm.RoomNumber,
                    Capacity = vm.Capacity
                };

                await _context.Classrooms.AddAsync(newClass);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }


        public IActionResult AddSection()
        {
            var vm = new SectionViewModel
            {
                Courses = _context.Courses
                                     .Select(c => new SelectListItem
                                     {
                                         Value = c.Id.ToString(),
                                         Text = c.Code + " – " + c.Name
                                     })
                                     .ToList(),

                Classrooms = _context.Classrooms
                                     .Select(r => new SelectListItem
                                     {
                                         Value = r.Id.ToString(),
                                         Text = r.Building + " " + r.RoomNumber
                                     })
                                     .ToList()
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // For security
        public async Task<IActionResult> AddSection(SectionViewModel vm)
        {
            // 1️) Re‑populate selects if we need to redisplay the form
            vm.Courses = _context.Courses.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Code + " – " + c.Name
            });
            vm.Classrooms = _context.Classrooms.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Building + " " + r.RoomNumber
            });

            if (!ModelState.IsValid)
                return View(vm);

            //// 2️) Optionally check for duplicates
            //bool exists = await _context.Sections.AnyAsync(s =>
            //    s.CourseId == vm.SelectedCourseId &&
            //    s.ClassroomId == vm.SelectedClassroomId &&
            //    s.Semester == vm.Semester &&
            //    s.Year == vm.Year
            //);

            //if (exists)
            //{
            //    ModelState.AddModelError("", "This section already exists for that course & classroom.");
            //    return View(vm);
            //}


            // 3️) Create and assign *all* the ID‐based FKs
            var newTimeSlot = new TimeSlots
            {
                Day = vm.Day,
                StartTime = vm.StartTime,
                EndTime = vm.EndTime
            };

            await _context.TimeSlots.AddAsync(newTimeSlot);
            await _context.SaveChangesAsync();

            var newSection = new Sections
            {
                Semester = vm.Semester,
                Year = vm.Year,
                CourseId = vm.SelectedCourseId,
                ClassroomId = vm.SelectedClassroomId,
                TimeSlotId = newTimeSlot.Id
            };

            await _context.Sections.AddAsync(newSection);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", nameof(Dashboard)); // Used nameof for compile-time safety
        }
    }
}