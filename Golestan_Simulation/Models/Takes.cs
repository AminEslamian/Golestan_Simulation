using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.Models
{
    /// <summary>
    /// this model is to join "Students" & "Sections"
    /// this model has:
    /// - "many to one" relationship: to "Students": as "dependent" side
    /// - "many to one" relationship: to "Sections": as "dependent" side
    /// </summary>
    [PrimaryKey(nameof(StudentId), nameof(SectionId))]
    public class Takes
    {
        public int StudentId { get; set; }
        public int SectionId { get; set; }
        public int Grade { get; set; }

        public Students Student { get; set; } = null!;                    //reference navigation
        public Sections Section { get; set; } = null!;
    }
}
