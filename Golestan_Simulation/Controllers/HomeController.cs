using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Golestan_Simulation.Models;
using Golestan_Simulation.Services;
using Golestan_Simulation.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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


    public IActionResult Login(RolesEnum role)
    {
        ViewBag.Role = role;
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
                return View(user);
            }

            if (_accountServices.IsPasswordCorrect(userRole, user.RawPassword))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userRole.User.UserName),
                    new Claim(ClaimTypes.Role, userRole.Role.Name.ToString()),
                    new Claim("UserId", userRole.UserId.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction(nameof(Index), "Instructor");
            }
            else
            {
                ModelState.AddModelError("Password", "The password is not correct");
                return View(user);
            }

        }
        return View(user);
    }


    public IActionResult AccessDenied()
    {
        return View();
    }
}
