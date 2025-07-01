using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Golestan_Simulation.Migrations
{
    /// <inheritdoc />
    public partial class Fluen1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RolesId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_RolesId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Teaches_InstructorId",
                table: "Teaches");

            migrationBuilder.DropIndex(
                name: "IX_Takes_StudentId",
                table: "Takes");

            migrationBuilder.DropIndex(
                name: "IX_Sections_CourseId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "RolesId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "Enrollment_Id",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "ExameDate",
                table: "Courses",
                newName: "ExamDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "TimeSlots",
                type: "date",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "TimeSlots",
                type: "date",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollmentDate",
                table: "Students",
                type: "date",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Sections",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "Salary",
                table: "Instructors",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HireDate",
                table: "Instructors",
                type: "date",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExamDate",
                table: "Courses",
                type: "date",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teaches",
                table: "Teaches",
                columns: new[] { "InstructorId", "SectionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Takes",
                table: "Takes",
                columns: new[] { "StudentId", "SectionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Courses_Id",
                table: "Sections",
                column: "Id",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Courses_Id",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teaches",
                table: "Teaches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Takes",
                table: "Takes");

            migrationBuilder.DropColumn(
                name: "EnrollmentDate",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "ExamDate",
                table: "Courses",
                newName: "ExameDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddColumn<int>(
                name: "RolesId",
                table: "UserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "TimeSlots",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "TimeSlots",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "Enrollment_Id",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Sections",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "Salary",
                table: "Instructors",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HireDate",
                table: "Instructors",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExameDate",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RolesId",
                table: "UserRoles",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_Teaches_InstructorId",
                table: "Teaches",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Takes_StudentId",
                table: "Takes",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CourseId",
                table: "Sections",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RolesId",
                table: "UserRoles",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
