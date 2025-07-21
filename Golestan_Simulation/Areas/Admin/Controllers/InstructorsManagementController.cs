using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.Services;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Golestan_Simulation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("Admin"))]
    public class InstructorsManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPassHasherService _passHasher;
        private readonly IRegisterationServices _registerServices;
        private readonly IAssignmentServices _assignmentServices;
        public InstructorsManagementController(ApplicationDbContext context, IPassHasherService passHasher, IRegisterationServices accountServices, IAssignmentServices assignment)
        {
            _context = context;
            _passHasher = passHasher;
            _registerServices = accountServices;
            _assignmentServices = assignment;
        }


        public async Task<IActionResult> Index()
        {
            var instructors = await _context.Instructors
                .Include(i => i.User)
                .AsNoTracking()
                .ToListAsync();

            return View(instructors);
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
                if (await _registerServices.IsUserNameAvailableAsync(instructorAccount.UserName))
                {
                    ModelState.AddModelError("UserName", "This user name is not available");
                    return View(instructorAccount);
                }
                if (await _registerServices.IsEmailAvailableAsync(instructorAccount.Email))
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

                return RedirectToAction("Index", nameof(DashboardController));
            }

            return View(instructorAccount);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var instructor = await _context.Instructors.Include(i => i.User).FirstOrDefaultAsync(i => i.Id == id);
            if (instructor == null) return NotFound();
            return View(instructor);   // renders Delete.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection form)
        {
            var instructor = await _context.Instructors.Include(i => i.User).FirstOrDefaultAsync(i => i.Id == id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

      
        public async Task<IActionResult> Edit(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor == null)
                return NotFound();

            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Salary,HireDate,User")] Instructors vm)
        {
            if (!ModelState.IsValid)
            {
                var original = await _context.Instructors
                    .Include(i => i.User)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (original == null)
                    return NotFound();

                original.Salary = vm.Salary;
                original.HireDate = vm.HireDate;
                return View(original);
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
                return NotFound();

            instructor.Salary = vm.Salary;
            instructor.HireDate = vm.HireDate;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Info(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.User) 
                .FirstOrDefaultAsync(i => i.Id == id);
            if (instructor == null)
                return NotFound();
            return View(instructor);
        }

        //// Optional: for confirmation page(if confirmed set the view page)
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var instructor = await _context.Instructors
        //        .FirstOrDefaultAsync(s => s.Id == id);

        //    if (instructor == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(instructor); // A confirmation view
        //}


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var instructor = await _context.Instructors
        //        .FirstOrDefaultAsync(s => s.Id == id);

        //    if (instructor == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Instructors.Remove(instructor);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}
    }
}
