using Microsoft.AspNetCore.Mvc;
using Golestan_Simulation.Data;
using Golestan_Simulation.ViewModels;
using Microsoft.EntityFrameworkCore;
using Golestan_Simulation.Models;
using Golestan_Simulation.Services;
using Microsoft.AspNetCore.Identity;

namespace Golestan_Simulation.Controllers
{
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
                if(await _accountServices.IsEmailAvailableAsync(instructorAccount.Email))
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
                if(role == null)
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
                if(await _accountServices.IsUserNameAvailableAsync(studentAccount.UserName))
                {
                    ModelState.AddModelError("UserName", "This user name is not available");
                    return View(studentAccount);
                }
                if(await _accountServices.IsEmailAvailableAsync(studentAccount.Email))
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
    }
}
