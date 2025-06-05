namespace Golestan_Simulation.Models
{
    public class UserRoles
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public Users User { get; set; } = null!;
        public Roles Role { get; set; } = null!;
    }
}
