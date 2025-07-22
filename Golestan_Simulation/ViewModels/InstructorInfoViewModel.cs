using Golestan_Simulation.Models;

namespace Golestan_Simulation.ViewModels
{
    public class InstructorInfoViewModel
    {
        public Instructors Instructor { get; set; }
        public List<AssignedSectionViewModel> AssignedSections { get; set; }
    }

    public class AssignedSectionViewModel
    {
        public int SectionId { get; set; }
        public int CourseId { get; set; }
        public int Semester { get; set; }
        public int Year { get; set; }
    }

}
