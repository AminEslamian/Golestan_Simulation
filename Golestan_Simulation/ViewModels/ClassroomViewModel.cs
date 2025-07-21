using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.ViewModels
{
    public class ClassroomViewModel
    {
        public string? Building {get; set; }
        public int RoomNumber { get; set; }
        [Range(0, 500)]
        public int Capacity { get; set; }

    }
}
