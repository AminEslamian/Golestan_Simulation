namespace Golestan_Simulation.Models
{
    /// <summary>
    /// this model has:
    /// - "many to one" relationship: to "Users": as "dependent" side
    /// - "one to many" realtionship: to "Teachs": as "principal" side
    /// </summary>
    public class Instructors
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }

        public Users User { get; set; } = null!;                          //navigation reference
        public ICollection<Teachs>? Teachs { get; set; }
    }
}
