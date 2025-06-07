namespace Golestan_Simulation.Models
{
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

        private ICollection<UserRoles>? Role { get; set; }
    }
}
