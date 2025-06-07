namespace Golestan_Simulation.Models
{
    public class Students
    {
        public int StudentId { set; get; }
        public int UserId { get; set; }
        public DateTime Enrollment_Id { get; set; }

        public Users User { get; set; } = null!;
        private ICollection<Takes>? Takes { get; set; }
    }
}
