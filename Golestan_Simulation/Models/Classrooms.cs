namespace Golestan_Simulation.Models
{
    public class Classrooms
    {
        public int Id { get; set; }
        public string? Building { get; set; }
        public int RoomNumber { get; set; }
        public int Capacity { get; set; }

        private ICollection<Sections>? Sections { get; set; }
    }
}
