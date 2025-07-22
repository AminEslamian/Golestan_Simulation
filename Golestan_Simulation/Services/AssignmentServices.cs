using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace Golestan_Simulation.Services
{
    public interface IAssignmentServices
    {
        Task<bool> InstructorHasTimeConflictAsync(int sectionId, int instructorId);
        Task<bool> StudentHasTimeConflict(int sectionId, int studentId);
        bool ClassroomIsFull(int sectionId);
    }




    public class AssignmentServices: IAssignmentServices
    {
        private readonly ApplicationDbContext _context;

        public AssignmentServices(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<bool> InstructorHasTimeConflictAsync(int sectionId, int instructorId)
        {
            var sectionTimeSlot = await _context.Sections.Where(s => s.Id == sectionId)
                .Include(s => s.TimeSlot)
                .Select(s => s.TimeSlot)
                .SingleAsync();
            
            var teachsTimeSlots = _context.Teaches.Where(t => t.InstructorId == instructorId)
                .Include(t => t.Section)
                .ThenInclude(s => s.TimeSlot)
                .Select(t => t.Section.TimeSlot);

            foreach(var time in teachsTimeSlots)
            {
                if (!time.Day.Equals(sectionTimeSlot.Day, StringComparison.CurrentCultureIgnoreCase))
                    return false;

                else if(sectionTimeSlot.StartTime > time.StartTime && sectionTimeSlot.StartTime < time.EndTime)
                    return true;

                else if(sectionTimeSlot.EndTime > time.StartTime && sectionTimeSlot.EndTime < time.EndTime)
                    return true;
            }

            return false;
        }

        public async Task<bool> StudentHasTimeConflict(int sectionId, int studentId)
        {
            var sectionTimeSlot = await _context.Sections.Where(s => s.Id == sectionId)
                .Include(s => s.TimeSlot)
                .Select(s => s.TimeSlot)
                .SingleAsync();

            var takesTimeSlots = _context.Takes.Where(t => t.StudentId == studentId)
                .Include(t => t.Section)
                .ThenInclude(s => s.TimeSlot)
                .Select(t => t.Section.TimeSlot);

            foreach (var time in takesTimeSlots)
            {
                if (!time.Day.Equals(sectionTimeSlot.Day, StringComparison.CurrentCultureIgnoreCase))
                    return false;

                else if (sectionTimeSlot.StartTime > time.StartTime && sectionTimeSlot.StartTime < time.EndTime)
                    return true;

                else if (sectionTimeSlot.EndTime > time.StartTime && sectionTimeSlot.EndTime < time.EndTime)
                    return true;
            }

            return false;
        }

        public bool ClassroomIsFull(int sectionId)
        {
            var classroom = _context.Sections
                .Where(s => s.Id == sectionId)
                .Include(s => s.Classroom)
                .Select(s => s.Classroom)
                .First();

            var takes = _context.Takes.Where(t => t.StudentId == sectionId);

            if(takes.Count() == classroom.Capacity)
                return true;
            else
                return false;
        }
    }
}
