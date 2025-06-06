namespace Golestan_Simulation.Models
{
    public class Takes
    {
        public int StudentId { get; set; }
        public int SectionId { get; set; }
        public int Grade { get; set; }

        public Students Student { get; set; } = null!;
        public Sections Section { get; set; } = null!;
    }
}
