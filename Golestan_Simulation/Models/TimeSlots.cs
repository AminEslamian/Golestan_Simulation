namespace Golestan_Simulation.Models
{
    public class TimeSlots
    {
        public int Id { get; set; }
        public string? Day { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        private ICollection<Sections>? Sections { get; set; }
    }
}
