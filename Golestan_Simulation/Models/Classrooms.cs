using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.Models
{
    /// <summary>
    /// this model has "one to many" relationship to "Sections" as "principal" side
    /// </summary>
    public class Classrooms
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string? Building { get; set; }
        public int RoomNumber { get; set; }
        public int Capacity { get; set; }

        public ICollection<Sections>? Sections { get; set; }             //reference navigaion
    }
}
