using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Golestan_Simulation.Services
{
    public interface IRegisterationServices
    {
        Task<bool> IsUserNameAvailableAsync(string userName);
        Task<bool> IsEmailAvailableAsync(string? email);
    }



    public class RegisterationServices: IRegisterationServices
    {
        private readonly ApplicationDbContext _context;
        public RegisterationServices(ApplicationDbContext context)
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
