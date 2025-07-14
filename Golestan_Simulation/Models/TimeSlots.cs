namespace Golestan_Simulation.Models
{
    /// <summary>
    /// this model has "one to many" relationship to "Sections" as "principal" side
    /// </summary>
    public class TimeSlots
    {
        public int Id { get; set; }
        public string? Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public ICollection<Sections>? Sections { get; set; }             //reference navigation
    }
}
