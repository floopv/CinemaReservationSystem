using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaReservationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnIsUsedToModelApplicationUserOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "ApplicationUserOTPs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "ApplicationUserOTPs");
        }
    }
}
