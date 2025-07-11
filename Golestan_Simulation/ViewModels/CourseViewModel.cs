using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.ViewModels
{
    public class CourseViewModel
    {
        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(20)]
        public string? Code { get; set; }
        [StringLength(2)]
        public string? Unit { get; set; } // Unit should probably be from int type; if changed, change in Course.cs too
        [StringLength(50)]
        public string? Description { get; set; }
        public DateOnly? ExamDate { get; set; } 
    }
}
