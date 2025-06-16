using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.Models
{
    /// <summary>
    /// this is to join "Users" & "Roles" and has:
    /// - "many to one" relationship: to "Users": as "dependent" side
    /// - "many to one" relationship: to "Roles": as "dependent" side
    /// </summary>
    // -- [Keyless]
    public class UserRoles
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public Users User { get; set; } = null!;                          //navigation reference
        public Roles Role { get; set; } = null!;
    }
}
