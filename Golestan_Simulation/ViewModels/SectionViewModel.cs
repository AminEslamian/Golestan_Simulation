using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.ViewModels
{
    public class SectionViewModel
    {
        //// for section:
        //public int Semester { get; set; }
        //public int Year { get; set; }
        //[StringLength(50)]
        //public string? CorseCode { get; set; }

        //// for classroom:
        //[StringLength(50)]
        //public string? Building { get; set; }
        //public int RoomNumber { get; set; }
        //public int Capacity { get; set; }

        //// for timeSlot:
        //[StringLength(50)]
        //public string? Day { get; set; }
        //public DateOnly? StartTime { get; set; }
        //public DateOnly? EndTime { get; set; }

        public int Semester { get; set; }
        public int Year { get; set; }

        // ← the two FK inputs
        public int SelectedCourseId { get; set; }
        public int SelectedClassroomId { get; set; }

        // ← data for rendering selects
        public IEnumerable<SelectListItem> Courses { get; set; }
        public IEnumerable<SelectListItem> Classrooms { get; set; }
    }
}
