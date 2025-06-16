using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.ViewModels
{
    public class InstructorAccountViewModel
    {
        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(50)]
        [Remote("IsUserNameAvailable", "Admin")]
        public string UserName { get; set; } = null!;

        [StringLength(50)]
        public string RawPassword { get; set; } = null!;

        [StringLength(50)]
        public string? Email { get; set; }

        [Range(10, 10000000000, ErrorMessage="the value is out of the valid range")]
        public decimal? Salary { get; set; }

        public DateOnly? HireDate { get; set; }
    }
}
