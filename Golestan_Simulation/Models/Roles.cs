namespace Golestan_Simulation.Models
{
    /// <summary>
    /// This model has "one to many" relationship with "UserRoles" as "principal" side
    /// </summary>
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

        public ICollection<UserRoles>? Roless { get; set; }                //navigation reference
    }
}
