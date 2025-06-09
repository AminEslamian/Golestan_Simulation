namespace Golestan_Simulation.Models
{
<<<<<<< HEAD
=======
    /// <summary>
    /// this model is to join "Courses", "Classrooms" & "TimeSlots"
    /// this model has:
    /// - "one to one" relationship: to "Courses": as "dependet" side
    /// - "many to one" relationship: to "Classrooms": as "dependent" side
    /// - "many to one" relationship: to "TimeSlots": as "dependent" side
    /// </summary>
>>>>>>> origin/master
    public class Sections
    {
        public int Id {  get; set; }
        public int CourseId { get; set; }
        public int Semester { get; set; }
        public int Year { get; set; }
        public int ClassroomId {  get; set; }
        public int TimeSlotId { get; set; }

<<<<<<< HEAD
        public Courses Course { get; set; } = null!;
        public Classrooms Classroom { get; set; } = null!;
        public TimeSlots TimeSlot { get; set; } = null!;
=======
        public Courses Course { get; set; } = null!;                      //reference navigation
        public Classrooms Classroom { get; set; } = null!;
        public TimeSlots TimeSlot { get; set; } = null!;
        private ICollection<Teachs>? Teachs { get; set; }
        private ICollection<Takes>? Takes { get; set; }
>>>>>>> origin/master
    }
}
