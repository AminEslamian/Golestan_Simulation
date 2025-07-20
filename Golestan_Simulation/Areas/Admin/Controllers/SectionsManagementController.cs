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
    public class SectionsManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAssignmentServices _assignmentServices;
        private readonly IAddSectionServices _sectionServices;
        public SectionsManagementController(ApplicationDbContext context, IAssignmentServices assignmentServices, IAddSectionServices sectionServices)
        {
            _context = context;
            _assignmentServices = assignmentServices;
            _sectionServices = sectionServices;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}


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

            // 3.1) Check for invalid user input

            if (!ModelState.IsValid)
                return View(vm);

            if (await _sectionServices.IsClassroomOccupied(vm))
            {
                ModelState.AddModelError("TimeSlot", "The classroom is occupied at the selected time!");
                return View(vm);
            }

            // 3️.2) Create and assign *all* the ID‐based FKs
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

            return RedirectToAction("Index", nameof(DashboardController)); // Used nameof for compile-time safety
        }


        public IActionResult AssignInstructorToSection(int sectionId)
        {
            var vm = new TeachsViewModel
            {
                SectionId = sectionId,
                Instructors = _context.Instructors.Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.User.FirstName} {i.User.LastName} _ {i.Id}"
                })
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AssignInstructorToSection(TeachsViewModel model)
        {
            if (await _assignmentServices.InstructorHasTimeConflictAsync(model.SectionId, model.InstructorId))
            {
                ModelState.AddModelError("InstructorId", "The instructor schedule has time conflict");
                return View(model);
            }

            var newTeachs = new Teachs
            {
                InstructorId = model.InstructorId,
                SectionId = model.SectionId
            };

            await _context.Teaches.AddAsync(newTeachs);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", nameof(DashboardController));
        }


        public IActionResult AssignStudentToSection(int sectionId)
        {
            var vm = new TakesViewModel
            {
                SectionId = sectionId,
                Students = _context.Students.Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.User.FirstName} {i.User.LastName} _ {i.Id}"
                })
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AssignStudentToSection(TakesViewModel model)
        {
            if (await _assignmentServices.StudentHasTimeConflict(model.SectionId, model.StudentId))
            {
                ModelState.AddModelError("StudentId", "The student schedule has time conflict");
                return View(model);
            }

            var newTakes = new Takes
            {
                StudentId = model.StudentId,
                SectionId = model.SectionId
            };

            await _context.Takes.AddAsync(newTakes);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", nameof(DashboardController));
        }
    }
}