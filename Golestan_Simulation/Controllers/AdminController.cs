using Microsoft.AspNetCore.Mvc;
using Golestan_Simulation.Data;
using Golestan_Simulation.ViewModels;
using Microsoft.EntityFrameworkCore;
using Golestan_Simulation.Models;
using Golestan_Simulation.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Golestan_Simulation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPassHasherService _passHasher;
        private readonly IUserAccountServices _accountServices;
        public AdminController(ApplicationDbContext context, IPassHasherService passHasher, IUserAccountServices accountServices)
        {
            _context = context;
            _passHasher = passHasher;
            _accountServices = accountServices;
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult RegisterInstructor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterInstructor(InstructorAccountViewModel instructorAccount)
        {
            if (ModelState.IsValid)
            {
                if (await _accountServices.IsUserNameAvailableAsync(instructorAccount.UserName))
                {
                    ModelState.AddModelError("UserName", "This user name is not available");
                    return View(instructorAccount);
                }
                if (await _accountServices.IsEmailAvailableAsync(instructorAccount.Email))
                {
                    ModelState.AddModelError("Email", "This email is not available");
                    return View(instructorAccount);
                }

                var newUser = new Users
                {
                    CreatedAt = DateTime.Now,
                    FirstName = instructorAccount.FirstName,
                    LastName = instructorAccount.LastName,
                    UserName = instructorAccount.UserName,
                    Email = instructorAccount.Email,
                    HashedPassword = _passHasher.HashPassword(instructorAccount.RawPassword)
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var role = _context.Roles.FirstOrDefault(r => r.Name == RolesEnum.Instructor);
                if (role == null)
                {
                    role = new Roles
                    {
                        Name = RolesEnum.Instructor
                    };
                    _context.Roles.Add(role);
                    await _context.SaveChangesAsync();
                }

                _context.UserRoles.Add(
                    new UserRoles
                    {
                        UserId = newUser.Id,
                        Role = role
                    }
                );

                _context.Instructors.Add(
                    new Instructors
                    {
                        UserId = newUser.Id,
                        Salary = instructorAccount.Salary,
                        HireDate = instructorAccount.HireDate
                    }
                );

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(instructorAccount);
        }


        public IActionResult RegisterStudent()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterStudent(StudentAccountViewModel studentAccount)
        {
            if (ModelState.IsValid)
            {
                if (await _accountServices.IsUserNameAvailableAsync(studentAccount.UserName))
                {
                    ModelState.AddModelError("UserName", "This user name is not available");
                    return View(studentAccount);
                }
                if (await _accountServices.IsEmailAvailableAsync(studentAccount.Email))
                {
                    ModelState.AddModelError("Email", "This email is not available");
                    return View(studentAccount);
                }

                var newUser = new Users
                {
                    CreatedAt = DateTime.Now,
                    FirstName = studentAccount.FirstName,
                    LastName = studentAccount.LastName,
                    UserName = studentAccount.UserName,
                    Email = studentAccount.Email,
                    HashedPassword = _passHasher.HashPassword(studentAccount.RawPassword)
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var role = _context.Roles.FirstOrDefault(r => r.Name == RolesEnum.Student);
                if (role == null)
                {
                    role = new Roles
                    {
                        Name = RolesEnum.Student
                    };
                    _context.Roles.Add(role);
                    await _context.SaveChangesAsync();
                }

                _context.UserRoles.Add(
                    new UserRoles
                    {
                        UserId = newUser.Id,
                        Role = role
                    }
                );

                _context.Students.Add(
                    new Students
                    {
                        UserId = newUser.Id,
                        EnrollmentDate = studentAccount.EnrollmentDate
                    }
                );

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentAccount);
        }

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

            return RedirectToAction(nameof(Index)); // Used nameof for compile-time safety
        }

    }
}
