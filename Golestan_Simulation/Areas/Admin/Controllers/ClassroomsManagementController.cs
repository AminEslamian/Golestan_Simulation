using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Golestan_Simulation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ClassroomsManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ClassroomsManagementController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var classrooms = await _context.Classrooms
                .AsNoTracking()
                .ToListAsync();

            return View(classrooms);
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


        // Optional: for confirmation page(if confirmed set the view page)
        public async Task<IActionResult> Delete(int id)
        {
            var classroom = await _context.Classrooms
                .FirstOrDefaultAsync(s => s.Id == id);

            if (classroom == null)
            {
                return NotFound();
            }

            return View(classroom); // A confirmation view
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classroom = await _context.Classrooms
                .FirstOrDefaultAsync(s => s.Id == id);

            if (classroom == null)
            {
                return NotFound();
            }

            _context.Classrooms.Remove(classroom);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var classroom = await _context.Classrooms
        //        .FirstOrDefaultAsync(s => s.Id == id);

        //    if (classroom == null)
        //    {
        //        return NotFound();
        //    }

        //    var vm = new ClassroomViewModel
        //    {
        //        Building = classroom.Building,
        //        Capacity = classroom.Capacity,
        //        RoomNumber = classroom.RoomNumber
        //    };
        //    return View(vm);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(ClassroomViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model); // Return same view with validation errors
        //    }

        //    var product = await _context.Classrooms.FindAsync(model);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    // Update properties
        //    product.Name = model.Name;
        //    product.Price = model.Price;

        //    _context.Update(product);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index)); // Redirect to list page or details page
        //}

    }
}
