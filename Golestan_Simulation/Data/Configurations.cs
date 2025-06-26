using Golestan_Simulation.Models;
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

            builder.HasKey(e => e.Id);

            builder.HasOne(s => s.User) // Student has one User
                .WithMany(u => u.Students) // User has many Students
                .HasForeignKey(s => s.UserId); // FK is on Student.UserId

            // ────────────────
            // 2) Property‑level Configuration
            // ────────────────

            builder.Property(e => e.EnrollmenDate)
                .HasColumnName("EnrollmentDate")
                .HasColumnType("date")
                .HasDefaultValueSql("getdate()");
        }
    }

}
