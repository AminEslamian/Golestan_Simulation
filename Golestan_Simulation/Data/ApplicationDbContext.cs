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
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Sections> Sections { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Takes> Takes { get; set; }
        public DbSet<Teachs> Teaches { get; set; }
        public DbSet<TimeSlots> TimeSlots { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Users> Users { get; set; }

        // WAY 1:
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.ApplyConfiguration(new StudentConfiguration());
        //    modelBuilder.ApplyConfiguration(new CourseConfiguration());
        //    ...

        //    base.OnModelCreating(modelBuilder);
        //}

        // WAY 2:
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // This will scan the current assembly for all IEntityTypeConfiguration<> implementations
        //    modelBuilder.ApplyConfigurationsFromAssembly(typeof(YourDbContext).Assembly);
        //    base.OnModelCreating(modelBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}