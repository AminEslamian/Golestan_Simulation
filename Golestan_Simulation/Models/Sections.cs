namespace Golestan_Simulation.Models
{
    public class Sections
    {
        public int Id {  get; set; }
        public int CourseId { get; set; }
        public int Semester { get; set; }
        public int Year { get; set; }
        public int ClassroomId {  get; set; }
        public int TimeSlotId { get; set; }

        public Courses Course { get; set; } = null!;
        public Classrooms Classroom { get; set; } = null!;
        public TimeSlots TimeSlot { get; set; } = null!;
        private ICollection<Teachs>? Teachs { get; set; }
        private ICollection<Takes>? Takes { get; set; }
    }
}
