namespace Golestan_Simulation.Models
{
<<<<<<< HEAD
=======
    /// <summary>
    /// this model has "one to many" relationship to "Sections" as "principal" side
    /// </summary>
>>>>>>> origin/master
    public class TimeSlots
    {
        public int Id { get; set; }
        public string? Day { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
<<<<<<< HEAD
=======

        private ICollection<Sections>? Sections { get; set; }             //reference navigation
>>>>>>> origin/master
    }
}
