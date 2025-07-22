using AspNetCoreGeneratedDocument;
using Golestan_Simulation.Data;
using Golestan_Simulation.Models;
using Golestan_Simulation.Services;
using Golestan_Simulation.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
                var userId = int.Parse(User.FindFirst("UserId").Value);
                var user = await _context.Users.FindAsync(userId);

                var model = new UserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    CreatedAt = DateTime.Now
                };

                var role = User.FindFirst(ClaimTypes.Role).Value;

                if(role == "Instructor")
                {
                    var intsructorId = int.Parse(User.FindFirst("DefaultInstructorId").Value);
                    var otherAccounts = _context.Instructors.Where(i => i.UserId == userId)
                    .Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.Id.ToString()
                    });

                    model.CurrentAccountId = intsructorId;
                    model.OtherAccountsOfThisUser = otherAccounts;
                }
                else if(role == "Student")
                {
                    var studentId = int.Parse(User.FindFirst("DefaultStudentId").Value);
                    var otherAccounts = _context.Students.Where(s => s.UserId == userId)
                    .Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.Id.ToString()
                    });

                    model.CurrentAccountId = studentId;
                    model.OtherAccountsOfThisUser = otherAccounts;
                }

                ViewBag.Role = role;

                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
        }


        public IActionResult Login(string? returnUrl = null)
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

            ViewBag.Role = expectedRole;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user, RolesEnum role)
        {
            if (ModelState.IsValid)
            {
                var userRole = await _authenticationServices.FindAccount(user, role);
                if (userRole == null)
                {
                    ModelState.AddModelError("UsernameOrEmail", "No account found with this user name or email");
                    ViewBag.Role = role;
                    return View(user);
                }

                if (_authenticationServices.IsPasswordCorrect(userRole, user.RawPassword))
                {
                    await _authenticationServices.AuthenticateUserAsync(userRole, HttpContext, null);

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


        [HttpPost]
        public async Task<IActionResult> SwitchAccount(int accountId)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);

            var userRole = await _context.UserRoles
                .Include(s => s.User)
                .Include(s => s.Role)
                .SingleAsync(ur => ur.User.Id == userId);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _authenticationServices.AuthenticateUserAsync(userRole, HttpContext, accountId);

            if (User.FindFirst(ClaimTypes.Role).Value == "Instructor")
                return RedirectToAction("Index", "Dashboard", new { area = "Instructor" });
            else if (User.FindFirst(ClaimTypes.Role).Value == "Student")
                return RedirectToAction("Index", "Dashboard", new { area = "Student" });

            return NotFound();
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