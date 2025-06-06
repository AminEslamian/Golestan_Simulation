namespace Golestan_Simulation.Models
{
    public class Teachs
    {
        public int InstructorId { get; set; }
        public int SectionId { get; set; }

        public Instructors Instructor { get; set; } = null!;
        public Sections Section { get; set; } = null!;
    }
}
