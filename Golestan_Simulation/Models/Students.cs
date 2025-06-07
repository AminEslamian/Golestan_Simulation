﻿namespace Golestan_Simulation.Models
{
    /// <summary>
    /// this model has:
    /// - "many to one" relationship: to "Users": as "dependent" side
    /// - "one to many" relationship: to "Takes": as "principal" side
    /// </summary>
    public class Students
    {
        public int StudentId { set; get; }
        public int UserId { get; set; }
        public DateTime Enrollment_Id { get; set; }

        public Users User { get; set; } = null!;                          //reference navigation
        private ICollection<Takes>? Takes { get; set; }
    }
}
