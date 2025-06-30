using System.ComponentModel.DataAnnotations;
namespace Golestan_Simulation.Models
{
    /// <summary>
    /// this model has:
    /// - "many to one" relationship: to "Users": as "dependent" side
    /// - "one to many" relationship: to "Takes": as "principal" side
    /// </summary>
    public class Students
    {
        public int Id { set; get; }
        public int UserId { get; set; }
        public DateOnly? EnrollmentDate { get; set; }

        public Users User { get; set; } = null!;                          //reference navigation
        private ICollection<Takes>? Takes { get; set; }
    }
}
