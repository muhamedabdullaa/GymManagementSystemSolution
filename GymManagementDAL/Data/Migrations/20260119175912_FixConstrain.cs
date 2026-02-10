using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementDAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixConstrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "GymUserValidPhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "GymUserValidPhoneCheck",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "GymUserValidPhoneCheck1",
                table: "Trainers",
                sql: "Phone Like '01_________' and Phone not Like '%[^0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "GymUserValidPhoneCheck",
                table: "Members",
                sql: "Phone Like '01_________' and Phone not Like '%[^0-9]%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "GymUserValidPhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "GymUserValidPhoneCheck",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "GymUserValidPhoneCheck1",
                table: "Trainers",
                sql: "Phone Like '01% and Phone not Like[^0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "GymUserValidPhoneCheck",
                table: "Members",
                sql: "Phone Like '01% and Phone not Like[^0-9]%'");
        }
    }
}
