namespace Golestan_Simulation.Models
{
<<<<<<< HEAD
=======
    /// <summary>
    /// this model has:
    /// - "many to one" relationship: to "Users": as "dependent" side
    /// - "one to many" realtionship: to "Teachs": as "principal" side
    /// </summary>
>>>>>>> origin/master
    public class Instructors
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }

<<<<<<< HEAD
        public Users User { get; set; } = null!;
=======
        public Users User { get; set; } = null!;                          //navigation reference
        private ICollection<Teachs>? Teachs { get; set; }
>>>>>>> origin/master
    }
}
