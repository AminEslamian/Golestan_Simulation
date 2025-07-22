using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.ViewModels
{
    public class InstructorViewModel
    {
        [Range(10, 10000000000)]
        [Required]
        public decimal Salary { get; set; }

        [Required]
        public DateOnly HireDate { get; set; }
    }
}
