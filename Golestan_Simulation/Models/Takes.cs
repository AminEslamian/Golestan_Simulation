namespace Golestan_Simulation.Models
{
<<<<<<< HEAD
=======
    /// <summary>
    /// this model is to join "Students" & "Sections"
    /// this model has:
    /// - "many to one" relationship: to "Students": as "dependent" side
    /// - "many to one" relationship: to "Sections": as "dependent" side
    /// </summary>
>>>>>>> origin/master
    public class Takes
    {
        public int StudentId { get; set; }
        public int SectionId { get; set; }
        public int Grade { get; set; }

<<<<<<< HEAD
        public Students Student { get; set; } = null!;
=======
        public Students Student { get; set; } = null!;                    //reference navigation
>>>>>>> origin/master
        public Sections Section { get; set; } = null!;
    }
}
