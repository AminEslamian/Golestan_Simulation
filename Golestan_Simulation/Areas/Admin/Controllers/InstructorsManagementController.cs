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


        public async Task<IActionResult> RegisterInstructor()
        {
            var model = new InstructorUserViewModel
            {
                ExistingInstructorUsers = _context.UserRoles
                .Include(ur => ur.Role).Include(ur => ur.User)
                .Where(ur => ur.Role.Name == RolesEnum.Instructor)
                .Select(ur => new SelectListItem
                {
                    Value = ur.UserId.ToString(),
                    Text = $"{ur.User.FirstName} {ur.User.LastName} - ({ur.User.UserName})"
                })
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterInstructor(InstructorUserViewModel model)
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
                            Salary = model.Instructor.Salary,
                            HireDate = model.Instructor.HireDate
                        }
                    );
                }
                else
                {
                    _context.Instructors.Add(
                        new Instructors
                        {
                            UserId = (int)model.SelectedUserId,
                            Salary = model.Instructor.Salary,
                            HireDate = model.Instructor.HireDate
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


        //public async Task<IActionResult> Edit(int id)
        //{
        //    var instructor = await _context.Instructors
        //        .Include(i => i.User)
        //        .FirstOrDefaultAsync(i => i.Id == id);

        //    if (instructor == null)
        //        return NotFound();

        //    return View(instructor);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(
        //    int id,
        //    [Bind("Salary,HireDate,User")] Instructors vm)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var original = await _context.Instructors
        //            .Include(i => i.User)
        //            .FirstOrDefaultAsync(i => i.Id == id);

        //        if (original == null)
        //            return NotFound();

        //        original.Salary = vm.Salary;
        //        original.HireDate = vm.HireDate;
        //        return View(original);
        //    }

        //    var instructor = await _context.Instructors.FindAsync(id);
        //    if (instructor == null)
        //        return NotFound();

        //    instructor.Salary = vm.Salary;
        //    instructor.HireDate = vm.HireDate;

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}



        public async Task<IActionResult> Info(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor == null)
                return NotFound();

            // Load the list of sections this instructor is assigned to
            var assignedSections = await _context.Teaches
                .Include(t => t.Section)
                .Where(t => t.InstructorId == id)
                .Select(t => new AssignedSectionViewModel
                {
                    SectionId = t.SectionId,
                    CourseId = t.Section.CourseId,
                    Semester = t.Section.Semester,
                    Year = t.Section.Year
                })
                .ToListAsync();

            var vm = new InstructorInfoViewModel
            {
                Instructor = instructor,
                AssignedSections = assignedSections
            };

            return View(vm);
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
