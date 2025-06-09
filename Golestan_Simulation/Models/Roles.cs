namespace Golestan_Simulation.Models
{
<<<<<<< HEAD
=======
    /// <summary>
    /// This model has "one to many" relationship with "UserRoles" as "principal" side
    /// </summary>
>>>>>>> origin/master
    public enum RolesEnum
    {
        None,
        Admin,
        Instructor,
        Student
    }
    public class Roles
    {
        public int Id { get; set; }
        public RolesEnum Name {get; set;}
<<<<<<< HEAD
=======

        private ICollection<UserRoles>? Role { get; set; }                //navigation reference
>>>>>>> origin/master
    }
}
