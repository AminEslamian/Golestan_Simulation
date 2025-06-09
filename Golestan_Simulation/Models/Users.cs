namespace Golestan_Simulation.Models
{
<<<<<<< HEAD
=======
    /// <summary>
    /// Users model have these relationships:
    /// - one to many: to "UserRoles": as "principal" side
    /// - one to many: to "Instructors": as "principal" side
    /// </summary>
>>>>>>> origin/master
    public class Users
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
<<<<<<< HEAD
=======

        private ICollection<Instructors>? Instructors { get; set; }       //navigation reference
        private ICollection<UserRoles>? Role { get; set; }
>>>>>>> origin/master
    }
}