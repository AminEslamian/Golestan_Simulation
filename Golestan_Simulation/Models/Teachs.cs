using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Golestan_Simulation.Models
{
    /// <summary>
    /// this model is to join "Instructors" & "Sections"
    /// this model has:
    /// - "many to one" relationship: to "Instructors: as "dependent" side
    /// - "many to one" relationship: to "Sections": as "dependent" side
    /// </summary>
    // -- [Keyless]
    public class Teachs
    {
        public int InstructorId { get; set; }
        public int SectionId { get; set; }

        public Instructors Instructor { get; set; } = null!;              //reference navigation
        public Sections Section { get; set; } = null!;
    }
}
