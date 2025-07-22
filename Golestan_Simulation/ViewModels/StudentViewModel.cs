using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.ViewModels
{
    public class StudentViewModel
    {
        [Required]
        public DateOnly EnrollmentDate { get; set; }
    }
}