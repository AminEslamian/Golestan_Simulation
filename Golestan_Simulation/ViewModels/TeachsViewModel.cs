using Microsoft.AspNetCore.Mvc.Rendering;

namespace Golestan_Simulation.ViewModels
{
    public class TeachsViewModel
    {
        public int InstructorId {  get; set; }
        public int SectionId {  get; set; }
        public IEnumerable<SelectListItem> Instructors { get; set; }
    }
}
