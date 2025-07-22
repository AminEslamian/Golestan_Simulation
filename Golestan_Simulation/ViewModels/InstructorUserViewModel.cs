using Microsoft.AspNetCore.Mvc.Rendering;

namespace Golestan_Simulation.ViewModels
{
    public class InstructorUserViewModel
    {
        public InstructorViewModel Instructor { get; set; }
        public UserViewModel? User { get; set; }
        public bool IsNewUser { get; set; }
        public int? SelectedUserId {  get; set; }
        public IEnumerable<SelectListItem>? ExistingInstructorUsers { get; set; }
    }
}
