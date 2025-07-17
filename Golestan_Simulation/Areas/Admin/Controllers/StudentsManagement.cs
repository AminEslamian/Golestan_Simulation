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
    public class StudentsManagement : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPassHasherService _passHasher;
        private readonly IRegisterationServices _registerServices;
        private readonly IAssignmentServices _assignmentServices;
        public StudentsManagement(ApplicationDbContext context, IPassHasherService passHasher, IRegisterationServices accountServices, IAssignmentServices assignment)
        {
            _context = context;
            _passHasher = passHasher;
            _registerServices = accountServices;
            _assignmentServices = assignment;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}


        public IActionResult RegisterStudent()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterStudent(StudentAccountViewModel studentAccount)
        {
            if (ModelState.IsValid)
            {
                if (await _registerServices.IsUserNameAvailableAsync(studentAccount.UserName))
                {
                    ModelState.AddModelError("UserName", "This user name is not available");
                    return View(studentAccount);
                }
                if (await _registerServices.IsEmailAvailableAsync(studentAccount.Email))
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
                return RedirectToAction("Index", nameof(Dashboard));
            }
            return View(studentAccount);
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

            return RedirectToAction("Index", nameof(Dashboard));
        }
    }
}
