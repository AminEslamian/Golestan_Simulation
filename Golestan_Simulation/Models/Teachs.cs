namespace Golestan_Simulation.Models
{
<<<<<<< HEAD
=======
    /// <summary>
    /// this model is to join "Instructors" & "Sections"
    /// this model has:
    /// - "many to one" relationship: to "Instructors: as "dependent" side
    /// - "many to one" relationship: to "Sections": as "dependent" side
    /// </summary>
>>>>>>> origin/master
    public class Teachs
    {
        public int InstructorId { get; set; }
        public int SectionId { get; set; }

<<<<<<< HEAD
        public Instructors Instructor { get; set; } = null!;
=======
        public Instructors Instructor { get; set; } = null!;              //reference navigation
>>>>>>> origin/master
        public Sections Section { get; set; } = null!;
    }
}
