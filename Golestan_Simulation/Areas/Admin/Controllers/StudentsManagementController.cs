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
                return RedirectToAction("Index", nameof(DashboardController));
            }
            return View(studentAccount);
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
            var st = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (st == null)
                return NotFound();
            return View(st);
        }
    }
}
