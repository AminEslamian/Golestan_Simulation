using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Golestan_Simulation.Services
{
    public interface IAddSectionServices
    {
        Task<bool> IsClassroomOccupied(SectionViewModel vm);
    }

    public class AddSectionServices : IAddSectionServices
    {
        private readonly ApplicationDbContext _context;
        public AddSectionServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsClassroomOccupied(SectionViewModel vm)
        {
            var occupied = await _context.Sections // Contains all the timeslots related to the current classroom
                .Where(s => s.ClassroomId == vm.SelectedClassroomId)
                .Select(s => s.TimeSlot)
                .ToListAsync();

            // If the year must be checked too, use the approch below instead
            //var occupied_ = _context.Sections 
            //    .Where(s => s.ClassroomId == vm.SelectedClassroomId)
            //    .Select(s => new
            //    {
            //        s.Year,
            //        s.TimeSlot.Day,
            //        s.TimeSlot.StartTime,
            //        s.TimeSlot.EndTime
            //    });

            foreach (TimeSlots t in occupied)
                if (vm.Day == t.Day)
                {
                    if (vm.EndTime < t.StartTime && vm.StartTime < t.StartTime)
                        return true;
                    else if (vm.EndTime > t.EndTime && vm.StartTime > t.EndTime)
                        return true;
                }
            return false;
        }
    }
}
