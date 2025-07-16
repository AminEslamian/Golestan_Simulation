using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Golestan_Simulation.Models;
using Golestan_Simulation.Services;
using Golestan_Simulation.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Golestan_Simulation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserAccountServices _accountServices;

    public HomeController(ILogger<HomeController> logger, IUserAccountServices accountServices)
    {
        _logger = logger;
        _accountServices = accountServices;
    }


    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    public IActionResult Login(RolesEnum role = RolesEnum.None, string? returnUrl = null)
    {
        if(role != RolesEnum.None)
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
    public async Task<IActionResult> Login(UserViewModel user, RolesEnum role)
    {
        if (ModelState.IsValid)
        {
            var userRole = await _accountServices.IsAccountExistingAsync(user, role);
            if (userRole == null)
            {
                ModelState.AddModelError("UsernameOrEmail", "No account found with this user name or email");
                ViewBag.Role = role;
                return View(user);
            }

            if (_accountServices.IsPasswordCorrect(userRole, user.RawPassword))
            {
                await _accountServices.AuthenticateUserAsync(userRole, HttpContext);

                if (role == RolesEnum.Instructor)
                    return RedirectToAction("Index", "Dashboard", new { area = "Instructor"});
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
