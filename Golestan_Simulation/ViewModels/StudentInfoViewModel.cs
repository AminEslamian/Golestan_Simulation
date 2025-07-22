using Golestan_Simulation.Models;

namespace Golestan_Simulation.ViewModels
{

        public class StudentInfoViewModel
        {
            public Students Student { get; set; }

            public List<AssignedSection> AssignedSections { get; set; }
        }

        public class AssignedSection
        {
            public int SectionId { get; set; }
            public string CourseDisplay { get; set; }    // e.g. "CS101 – Intro to AI"
            public int Semester { get; set; }
            public int Year { get; set; }
            public string ClassroomDisplay { get; set; } // e.g. "Bldg A 101"
            public string TimeSlotDisplay { get; set; }  // e.g. "Mon 09:00–11:00"
        }
    }

