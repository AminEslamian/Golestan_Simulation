namespace Golestan_Simulation.Models
{
<<<<<<< HEAD
=======
    /// <summary>
    /// this model has "one to many" relationship to "Sections" as "principal" side
    /// </summary>
>>>>>>> origin/master
    public class Classrooms
    {
        public int Id { get; set; }
        public string? Building { get; set; }
        public int RoomNumber { get; set; }
        public int Capacity { get; set; }
<<<<<<< HEAD
=======

        private ICollection<Sections>? Sections { get; set; }             //reference navigaion
>>>>>>> origin/master
    }
}
