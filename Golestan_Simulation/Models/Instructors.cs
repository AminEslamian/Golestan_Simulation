namespace Golestan_Simulation.Models
{
    public class Instructors
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }

        public Users User { get; set; } = null!;
        private ICollection<Teachs>? Teachs { get; set; }
    }
}
