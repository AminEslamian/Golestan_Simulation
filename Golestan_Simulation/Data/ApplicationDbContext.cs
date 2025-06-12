using Golestan_Simulation.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Golestan_Simulation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Classrooms> Classrooms { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Instructors> Instructors { get; set; }
        // public DbSet<Roles> Roles { get; set; } // Probably not needed, but wasn't sure!
        public DbSet<Sections> Sections { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Takes> Takes { get; set; }
        public DbSet<Teachs> Teaches { get; set; }
        public DbSet<TimeSlots> TimeSlots { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Users> Users { get; set; }

    }
}