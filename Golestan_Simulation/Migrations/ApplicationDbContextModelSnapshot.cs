﻿// <auto-generated />
using System;
using Golestan_Simulation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Golestan_Simulation.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Golestan_Simulation.Models.Classrooms", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Building")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("Building");

                    b.Property<int>("Capacity")
                        .HasColumnType("int")
                        .HasColumnName("Capacity");

                    b.Property<int>("RoomNumber")
                        .HasColumnType("int")
                        .HasColumnName("RoomNumber");

                    b.HasKey("Id");

                    b.ToTable("Classrooms", (string)null);
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Courses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Code");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Description");

                    b.Property<DateOnly?>("ExameDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("ExamDate")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Unit");

                    b.HasKey("Id");

                    b.ToTable("Courses", (string)null);
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Instructors", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly?>("HireDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("HireDate")
                        .HasDefaultValueSql("getdate()");

                    b.Property<decimal?>("Salary")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("Salary");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Instructors", (string)null);
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Name")
                        .HasColumnType("int")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Sections", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClassroomId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("Semester")
                        .HasColumnType("int")
                        .HasColumnName("Semester");

                    b.Property<int>("TimeSlotId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int")
                        .HasColumnName("Year");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomId");

                    b.HasIndex("CourseId");

                    b.HasIndex("TimeSlotId");

                    b.ToTable("Sections", (string)null);
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Students", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly?>("EnrollmentDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("EnrollmentDate")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Students", (string)null);
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Takes", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.Property<int>("Grade")
                        .HasColumnType("int")
                        .HasColumnName("Grade");

                    b.HasKey("StudentId", "SectionId");

                    b.HasIndex("SectionId");

                    b.ToTable("Takes");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Teachs", b =>
                {
                    b.Property<int>("InstructorId")
                        .HasColumnType("int");

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.HasKey("InstructorId", "SectionId");

                    b.HasIndex("SectionId");

                    b.ToTable("Teaches");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.TimeSlots", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Day")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Day");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time")
                        .HasColumnName("EndTime");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time")
                        .HasColumnName("StartTime");

                    b.HasKey("Id");

                    b.ToTable("TimeSlots", (string)null);
                });

            modelBuilder.Entity("Golestan_Simulation.Models.UserRoles", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("CreatedAt")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Email");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("FirstName");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("HashedPassword");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("LastName");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Instructors", b =>
                {
                    b.HasOne("Golestan_Simulation.Models.Users", "User")
                        .WithMany("Instructors")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Sections", b =>
                {
                    b.HasOne("Golestan_Simulation.Models.Classrooms", "Classroom")
                        .WithMany("Sections")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Golestan_Simulation.Models.Courses", "Course")
                        .WithMany("Sections")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Golestan_Simulation.Models.TimeSlots", "TimeSlot")
                        .WithMany("Sections")
                        .HasForeignKey("TimeSlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classroom");

                    b.Navigation("Course");

                    b.Navigation("TimeSlot");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Students", b =>
                {
                    b.HasOne("Golestan_Simulation.Models.Users", "User")
                        .WithMany("Students")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Takes", b =>
                {
                    b.HasOne("Golestan_Simulation.Models.Sections", "Section")
                        .WithMany("Takes")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Golestan_Simulation.Models.Students", "Student")
                        .WithMany("Takes")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Section");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Teachs", b =>
                {
                    b.HasOne("Golestan_Simulation.Models.Instructors", "Instructor")
                        .WithMany("Teachs")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Golestan_Simulation.Models.Sections", "Section")
                        .WithMany("Teachs")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instructor");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.UserRoles", b =>
                {
                    b.HasOne("Golestan_Simulation.Models.Roles", "Role")
                        .WithMany("Roless")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Golestan_Simulation.Models.Users", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Classrooms", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Courses", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Instructors", b =>
                {
                    b.Navigation("Teachs");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Roles", b =>
                {
                    b.Navigation("Roless");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Sections", b =>
                {
                    b.Navigation("Takes");

                    b.Navigation("Teachs");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Students", b =>
                {
                    b.Navigation("Takes");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.TimeSlots", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("Golestan_Simulation.Models.Users", b =>
                {
                    b.Navigation("Instructors");

                    b.Navigation("Roles");

                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
