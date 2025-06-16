using Golestan_Simulation.Models;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Golestan_Simulation.Data
{
    public class StudentConfiguration : IEntityTypeConfiguration<Students>
    {
        public void Configure(EntityTypeBuilder<Students> builder)
        {
            // ────────────────
            // 1) Entity‑level Configuration
            // ────────────────

            builder.ToTable("Students");

            builder.HasKey(e => e.StudentId);

            builder.HasOne(s => s.User) // Student has one User
                .WithMany(u => u.Students) // User has many Students
                .HasForeignKey(s => s.UserId); // FK is on Student.UserId

            // ────────────────
            // 2) Property‑level Configuration
            // ────────────────

            builder.Property(e => e.EnrollmentDate) // ## This approch might need to be reconsidered! ##
                .HasColumnName("EnrollmentDate")
                .HasColumnType("date")
                .HasDefaultValueSql("getdate()");

            // ### Takes prperty is left unconfigured! ###
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builer)
        {
            // ────────────────
            // 1) Entity‑level Configuration
            // ────────────────

            builer.ToTable("Users");
            builer.HasKey(e => e.Id);

            // ─────────── Students relationship ───────────

            builer.HasMany(u => u.Students)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            // ─────────── Instructor relationship ───────────
            builer.HasMany(u => u.Instructors)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.Id);

            // ─────────── User roles relationship ───────────
            // ### is it needed to add or not?


            // ────────────────
            // 2) Property‑level Configuration
            // ────────────────

            builer.Property(e => e.FirstName)
                .HasColumnName("FirstName");
            builer.Property(e => e.LastName)
                .HasColumnName("LastName");
            builer.Property(e => e.Email)
                .HasColumnName("Email");
            builer.Property(e => e.HashedPassword)
                .HasColumnName("HashedPassword");
            builer.Property(e => e.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("date")
                .HasDefaultValueSql("getdate()");

        }
    }

    public class InstructorConfiguration : IEntityTypeConfiguration<Instructors>
    {
        public void Configure(EntityTypeBuilder<Instructors> builder)
        {
            builder.ToTable("Instructors");
            builder.HasKey(e => e.Id);

            builder.HasOne(i => i.User) 
                .WithMany(u => u.Instructors) 
                .HasForeignKey(i => i.UserId); 
            

            builder.Property(e => e.HireDate)
                .HasColumnName("HireDate")
                .HasColumnType("date")
                .HasDefaultValueSql("getdate()");
            builder.Property(e => e.Salary)
                .HasColumnName("Salary")
                .HasColumnType("decimal(10,2)");

            // ### Teaches prperty is left unconfigured! ###
        }
    }

    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            // The composite primar key:
            builder.HasKey(e => new { e.RoleId, e.UserId });

            builder.HasOne(ur => ur.User)
                .WithMany(u => u.Role) // Change User.Role propery to User.Roles
                .HasForeignKey(ur => ur.UserId);
            builder.HasOne(ur => ur.Role)
                .WithMany(u => u.Roless) // Change Roles.Role to Roles.Role
                .HasForeignKey(ur => ur.RoleId);
            
        }
    }
}
