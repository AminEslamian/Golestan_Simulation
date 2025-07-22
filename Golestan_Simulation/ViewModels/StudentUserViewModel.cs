using Microsoft.AspNetCore.Mvc.Rendering;

namespace Golestan_Simulation.ViewModels
{
    public class StudentUserViewModel
    {
        public StudentViewModel Student { get; set; } = null!;
        public UserViewModel? User { get; set; }
        public bool IsNewUser {  get; set; }
        public int? SelectedUserId {  get; set; }
        public IEnumerable<SelectListItem>? ExistingStudentUsers { get; set; }
    }
}
