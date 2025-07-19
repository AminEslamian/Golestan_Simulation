using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.ViewModels
{
    public class SectionViewModel
    {
        [Range(0, 20, ErrorMessage = "The chosen semester is not valid!")]
        public int Semester { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Selected year must be positive!")]
        public int Year { get; set; }

        // the two FK inputs:
        public int SelectedCourseId { get; set; }
        public int SelectedClassroomId { get; set; }

        // data for rendering selects:
        public IEnumerable<SelectListItem>? Courses { get; set; }
        public IEnumerable<SelectListItem>? Classrooms { get; set; }

        // for time-slot:
        public string? Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }
}
