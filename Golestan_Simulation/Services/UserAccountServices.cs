using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Golestan_Simulation.Services
{
    public interface IUserAccountServices
    {
        Task<bool> IsUserNameAvailableAsync(string userName);
        Task<bool> IsEmailAvailableAsync(string? email);
        Task<UserRoles?> IsAccountExistingAsync(UserViewModel account, RolesEnum roleName);
        bool IsPasswordCorrect(UserRoles userRoles, string rawPass);
    }


    public class UserAccountServices: IUserAccountServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IPassHasherService _passHasher;
        public UserAccountServices(ApplicationDbContext context, IPassHasherService passHasher)
        {
            _context = context;
            _passHasher = passHasher;
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
        public async Task<UserRoles?> IsAccountExistingAsync(UserViewModel account, RolesEnum roleName)
        {
            try
            {
                var ur = await _context.UserRoles.Include(s => s.User).Include(s => s.Role)
                    .Where(ur => ur.Role.Name == roleName)
                    .SingleAsync(ur => ur.User.UserName == account.UsernameOrEmail || ur.User.Email == account.UsernameOrEmail);
                
                return ur;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            
        }
        public bool IsPasswordCorrect(UserRoles ur, string rawPass)
        {
            var hashedPass = _passHasher.HashPassword(rawPass);

            var isCorrect = ur.User.HashedPassword.Equals(hashedPass);

            return isCorrect;
        }
    }
}
