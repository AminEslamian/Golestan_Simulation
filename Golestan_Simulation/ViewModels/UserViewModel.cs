using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.ViewModels
{
    public class UserViewModel
    {
        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(50)]
        public string UserName { get; set; } = null!;

        [StringLength(50)]
        public string RawPassword { get; set; } = null!;

        [StringLength(50)]
        public string? Email { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
