namespace Golestan_Simulation.Models
{
<<<<<<< HEAD
=======
    /// <summary>
    /// this is to join "Users" & "Roles" and has:
    /// - "many to one" relationship: to "Users": as "dependent" side
    /// - "many" relationship: to "Roles": as "dependent" side
    /// </summary>
>>>>>>> origin/master
    public class UserRoles
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

<<<<<<< HEAD
        public Users User { get; set; } = null!;
=======
        public Users User { get; set; } = null!;                          //navigation reference
>>>>>>> origin/master
        public Roles Roles { get; set; } = null!;
    }
}
