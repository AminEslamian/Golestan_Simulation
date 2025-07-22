using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.ViewModels
{
    public class InstructorViewModel
    {
        [Range(10, 10000000000, ErrorMessage="the value is out of the valid range")]
        public decimal? Salary { get; set; }

        public DateOnly? HireDate { get; set; }
    }
}
