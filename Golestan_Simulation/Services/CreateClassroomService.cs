using Golestan_Simulation.Data;
using Microsoft.EntityFrameworkCore;

namespace Golestan_Simulation.Services
{
    public interface ICreateClassroomService
    {
        Task<bool> IsClassroomAvailableAsync(string? Building, int RoomNumber);
    }

    public class CreateClassroomService : ICreateClassroomService
    {
        private readonly ApplicationDbContext _context;
        public CreateClassroomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsClassroomAvailableAsync(string? Building, int RoomNumber)
        {
            bool exists = await _context.Classrooms
        .AnyAsync(c => c.Building == Building
                    && c.RoomNumber == RoomNumber);

            return exists;
        }
    }
}
