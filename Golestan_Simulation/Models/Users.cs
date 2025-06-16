namespace Golestan_Simulation.Models
{
    /// <summary>
    /// Users model have these relationships:
    /// - one to many: to "UserRoles": as "principal" side
    /// - one to many: to "Instructors": as "principal" side
    /// - one to many: to "Students": as "principal" side
    /// </summary>
    public class Users
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }

        private ICollection<Instructors>? Instructors { get; set; }       //navigation reference
        private ICollection<UserRoles>? Role { get; set; }
        public ICollection<Students>? Students { get; set; }
    }
}