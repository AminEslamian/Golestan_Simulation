using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.ViewModels
{
    public class UserViewModel
    {
        public DateTime CreatedAt { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string UserName { get; set; } = null!;

        public string? Email { get; set; }
    }
}
