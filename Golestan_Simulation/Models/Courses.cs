namespace Golestan_Simulation.Models
{
    /// <summary>
    /// this model has "one to one" relationship to "Sections" as "principal" side
    /// </summary>
    public class Courses
    {
        public int Id { get; set;}
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Unit { get; set; }
        public string? Description { get; set; }
        public DateTime ExameDate { get; set; }
    }
}
