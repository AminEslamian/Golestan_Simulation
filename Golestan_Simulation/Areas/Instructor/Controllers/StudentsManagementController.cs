using Golestan_Simulation.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Golestan_Simulation.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class StudentsManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsManagementController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
