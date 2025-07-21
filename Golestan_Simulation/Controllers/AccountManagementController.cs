using AspNetCoreGeneratedDocument;
using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.Services;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Golestan_Simulation.Controllers
{
    public class AccountManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGolestanAuthenticationServices _authenticationServices;
        public AccountManagementController(IGolestanAuthenticationServices authenticationServices, ApplicationDbContext context)
        {
            _context = context;
            _authenticationServices = authenticationServices;
        }


        public async Task<IActionResult> Index()
        {
            if(User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst("UserId").Value;
                var user = await _context.Users.FindAsync(int.Parse(userId));

                var model = new UserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    CreatedAt = DateTime.Now
                };

                ViewBag.Role = User.FindFirst(ClaimTypes.Role).Value;

                return View(model);
            }
            else
            {
                return View();
            }
        }


        public IActionResult Login(RolesEnum role = RolesEnum.None, string? returnUrl = null)
        {
            if (role != RolesEnum.None)
            {
                ViewBag.Role = role;
            }
            else
            {
                RolesEnum expectedRole = RolesEnum.None;
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    if (returnUrl.StartsWith("/admin", StringComparison.OrdinalIgnoreCase))
                        expectedRole = RolesEnum.Admin;
                    else if (returnUrl.StartsWith("/instructor", StringComparison.OrdinalIgnoreCase))
                        expectedRole = RolesEnum.Instructor;
                    else if (returnUrl.StartsWith("/student", StringComparison.OrdinalIgnoreCase))
                        expectedRole = RolesEnum.Student;
                }
                else
                    return NotFound();

                ViewBag.Role = expectedRole;
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user, RolesEnum role)
        {
            if (ModelState.IsValid)
            {
                var userRole = await _authenticationServices.IsAccountExistingAsync(user, role);
                if (userRole == null)
                {
                    ModelState.AddModelError("UsernameOrEmail", "No account found with this user name or email");
                    ViewBag.Role = role;
                    return View(user);
                }

                if (_authenticationServices.IsPasswordCorrect(userRole, user.RawPassword))
                {
                    await _authenticationServices.AuthenticateUserAsync(userRole, HttpContext);

                    if (role == RolesEnum.Instructor)
                        return RedirectToAction("Index", "Dashboard", new { area = "Instructor" });
                    else if (role == RolesEnum.Student)
                        return RedirectToAction("Index", "Dashboard", new { area = "Student" });
                    else if (role == RolesEnum.Admin)
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                }
                else
                {
                    ModelState.AddModelError("RawPassword", "The password is not correct");
                    ViewBag.Role = role;
                    return View(user);
                }

            }
            return View(user);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}