using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.Models
{
    /// <summary>
    /// Users model have these relationships:
    /// - one to many: to "UserRoles": as "principal" side
    /// - one to many: to "Instructors": as "principal" side
    /// </summary>
    public class Users
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(50)]
        public string UserName { get; set; } = null!;

        [StringLength(50)]
        public string? Email { get; set; }
        public string HashedPassword { get; set; } = null!;

        private ICollection<Instructors>? Instructors { get; set; }       //navigation reference
        private ICollection<UserRoles>? Role { get; set; }
    }
}