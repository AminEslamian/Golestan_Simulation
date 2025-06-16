using Golestan_Simulation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Golestan_Simulation.Services
{
    public interface IUserAccountServices
    {
        Task<bool> IsUserNameAvailableAsync(string userName);
        Task<bool> IsEmailAvailableAsync(string userName);
    }


    public class UserAccountServices: IUserAccountServices
    {
        private readonly ApplicationDbContext _context;
        public UserAccountServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsUserNameAvailableAsync(string userName)
        {
            var isAvailable = await _context.Users.AnyAsync(u => u.UserName == userName);

            return isAvailable;
        }
        public async Task<bool> IsEmailAvailableAsync(string? email)
        {
            var isAvailable = await _context.Users.AnyAsync(u => u.Email == email);

            return isAvailable;
        }
    }
}
