using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Golestan_Simulation.Services
{
    public interface IGolestanAuthenticationServices
    {
        Task<UserRoles?> IsAccountExistingAsync(UserViewModel account, RolesEnum roleName);
        bool IsPasswordCorrect(UserRoles userRoles, string rawPass);
        Task AuthenticateUserAsync(UserRoles userRole, HttpContext httpContext);
    }



    public class GolestanAuthenticationServices: IGolestanAuthenticationServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IPassHasherService _passHasher;
        public GolestanAuthenticationServices(ApplicationDbContext context, IPassHasherService passHasher)
        {
            _context = context;
            _passHasher = passHasher;
        }

        public async Task<UserRoles?> IsAccountExistingAsync(UserViewModel account, RolesEnum roleName)
        {
            try
            {
                if (roleName == RolesEnum.Student)
                {
                    var ur = await _context.UserRoles
                    .Where(ur => ur.Role.Name == roleName)
                    .Include(s => s.User).ThenInclude(u => u.Students)
                    .Include(s => s.Role)
                    .SingleAsync(ur => ur.User.UserName == account.UsernameOrEmail || ur.User.Email == account.UsernameOrEmail);

                    return ur;
                }
                else if (roleName == RolesEnum.Instructor)
                {
                    var ur = await _context.UserRoles
                    .Where(ur => ur.Role.Name == roleName)
                    .Include(s => s.User).ThenInclude(u => u.Instructors)
                    .Include(s => s.Role)
                    .SingleAsync(ur => ur.User.UserName == account.UsernameOrEmail || ur.User.Email == account.UsernameOrEmail);

                    return ur;
                }
                else if (roleName == RolesEnum.Admin)
                {
                    var ur = await _context.UserRoles
                    .Where(ur => ur.Role.Name == roleName)
                    .Include(s => s.User)
                    .Include(s => s.Role)
                    .SingleAsync(ur => ur.User.UserName == account.UsernameOrEmail || ur.User.Email == account.UsernameOrEmail);

                    return ur;
                }
                else
                    return null;
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
        public async Task AuthenticateUserAsync(UserRoles userRole, HttpContext httpContext)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userRole.User.UserName),
                    new Claim(ClaimTypes.Role, userRole.Role.Name.ToString()),
                    new Claim("UserId", userRole.UserId.ToString())
                };

            if (userRole.Role.Name == RolesEnum.Student)
            {
                var defaultStudentId = userRole.User.Students.First().Id;

                claims.Add(new Claim("DefaultStudentId", defaultStudentId.ToString()));
            }
            else if (userRole.Role.Name == RolesEnum.Instructor)
            {
                var defaultInstructorId = userRole.User.Instructors.First().Id;

                claims.Add(new Claim("DefaultInstructorId", defaultInstructorId.ToString()));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
