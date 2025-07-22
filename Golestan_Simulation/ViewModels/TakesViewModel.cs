using Microsoft.AspNetCore.Mvc.Rendering;

namespace Golestan_Simulation.ViewModels
{
    public class TakesViewModel
    {
        public int StudentId {  get; set; }
        public int SectionId {  get; set; }
        public IEnumerable<SelectListItem>? Students { get; set; }
    }
}