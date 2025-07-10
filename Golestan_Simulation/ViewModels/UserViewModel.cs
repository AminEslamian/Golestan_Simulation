using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.ViewModels
{
    public class UserViewModel
    {
        [StringLength(50)]
        public string UsernameOrEmail { get; set; } = null!;

        [StringLength(50)]
        public string RawPassword { get; set; } = null!;
    }
}
