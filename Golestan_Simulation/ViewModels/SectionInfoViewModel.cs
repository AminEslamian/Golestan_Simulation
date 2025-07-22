using Golestan_Simulation.Models;

namespace Golestan_Simulation.ViewModels
{
    public class SectionInfoViewModel
    {
        public Sections Section { get; set; }

        public List<EnrolledStudent> EnrolledStudents { get; set; }
    }

    public class EnrolledStudent
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
    }

}
