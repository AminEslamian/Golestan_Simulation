using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.Services;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Golestan_Simulation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StudentsManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPassHasherService _passHasher;
        private readonly IRegisterationServices _registerServices;
        public StudentsManagementController(ApplicationDbContext context, IPassHasherService passHasher, IRegisterationServices accountServices)
        {
            _context = context;
            _passHasher = passHasher;
            _registerServices = accountServices;
        }


        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .Include(s => s.User)
                .AsNoTracking()
                .ToListAsync();

            return View(students);
        }


        public IActionResult RegisterStudent()
        {
            var model = new StudentUserViewModel
            {
                ExistingStudentUsers = _context.UserRoles
                .Include(ur => ur.Role).Include(ur => ur.User)
                .Where(ur => ur.Role.Name == RolesEnum.Student)
                .Select(ur => new SelectListItem
                {
                    Value = ur.UserId.ToString(),
                    Text = $"{ur.User.FirstName} {ur.User.LastName} - ({ur.User.UserName})"
                })
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterStudent(StudentUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.IsNewUser == true)
                {
                    if (await _registerServices.IsUserNameAvailableAsync(model.User.UserName))
                    {
                        ModelState.AddModelError("User.UserName", "This user name is not available");
                        return View(model);
                    }
                    if (await _registerServices.IsEmailAvailableAsync(model.User.Email))
                    {
                        ModelState.AddModelError("User.Email", "This email is not available");
                        return View(model);
                    }

                    var newUser = new Users
                    {
                        CreatedAt = DateTime.Now,
                        FirstName = model.User.FirstName,
                        LastName = model.User.LastName,
                        UserName = model.User.UserName,
                        Email = model.User.Email,
                        HashedPassword = _passHasher.HashPassword(model.User.RawPassword)
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
                            EnrollmentDate = model.Student.EnrollmentDate
                        }
                    );
                }
                else
                {
                    _context.Students.Add(
                        new Students
                        {
                            UserId = (int)model.SelectedUserId,
                            EnrollmentDate = model.Student.EnrollmentDate
                        }
                    );
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var st = await _context.Students.FindAsync(id);
            if (st == null) return NotFound();
            return View(st);   // renders Delete.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection form)
        {
            var st = await _context.Students.FindAsync(id);
            if (st != null)
            {
                _context.Students.Remove(st);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var st = await _context.Students.FindAsync(id);
        //    if (st == null)
        //        return NotFound();

        //    return View(st);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(
        //    int id,
        //    Students vm)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var original = await _context.Students.FindAsync(id);
        //        if (original == null)
        //            return NotFound();

        //        original.EnrollmentDate = vm.EnrollmentDate;
        //    }

        //    // Now perform the real update
        //    var st = await _context.Students.FindAsync(id);
        //    if (st == null)
        //        return NotFound();
        //    st.EnrollmentDate = vm.EnrollmentDate;

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        public async Task<IActionResult> Info(int id)
        {
            // 1) load the student + their User
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound();

            // 2) load all takes for that student, including the Section → Course, Classroom, TimeSlot
            var takes = await _context.Takes
                .Where(t => t.StudentId == id)
                .Include(t => t.Section)
                    .ThenInclude(s => s.Course)
                .Include(t => t.Section)
                    .ThenInclude(s => s.Classroom)
                .Include(t => t.Section)
                    .ThenInclude(s => s.TimeSlot)
                .ToListAsync();

            // 3) project into our VM
            var vm = new StudentInfoViewModel
            {
                Student = student,
                AssignedSections = takes.Select(t => new AssignedSection
                {
                    SectionId = t.SectionId,
                    CourseDisplay = $"{t.Section.Course.Code} – {t.Section.Course.Name}",
                    Semester = t.Section.Semester,
                    Year = t.Section.Year,
                    ClassroomDisplay = $"{t.Section.Classroom.Building} {t.Section.Classroom.RoomNumber}",
                    TimeSlotDisplay = $"{t.Section.TimeSlot.Day} {t.Section.TimeSlot.StartTime:hh\\:mm}–{t.Section.TimeSlot.EndTime:hh\\:mm}"
                })
                .ToList()
            };

            return View(vm);
        }
    }
}
