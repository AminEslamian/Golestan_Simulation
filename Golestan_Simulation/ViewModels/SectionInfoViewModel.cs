using Golestan_Simulation.Models;

namespace Golestan_Simulation.ViewModels
{
    public class SectionInfoViewModel
    {
        public Sections Section { get; set; }

        public List<EnrolledStudent> EnrolledStudents { get; set; }
        public EnrolledInstructor AssignedInstructor { get; set; }
    }

    public class EnrolledStudent
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
    }

    public class EnrolledInstructor
    {
        public int InstructorId { get; set; }
        public string FullName { get; set; }
    }

}
