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
    [Authorize(Roles = ("Admin"))]
    public class InstructorsManagement : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPassHasherService _passHasher;
        private readonly IUserAccountServices _accountServices;
        private readonly IAssignmentServices _assignmentServices;
        public InstructorsManagement(ApplicationDbContext context, IPassHasherService passHasher, IUserAccountServices accountServices, IAssignmentServices assignment)
        {
            _context = context;
            _passHasher = passHasher;
            _accountServices = accountServices;
            _assignmentServices = assignment;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}


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

                return RedirectToAction("Index", nameof(Dashboard));
            }

            return View(instructorAccount);
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

            return RedirectToAction("Index", nameof(Dashboard));
        }
    }
}
