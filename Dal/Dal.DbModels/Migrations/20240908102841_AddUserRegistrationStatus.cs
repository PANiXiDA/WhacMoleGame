using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.DbModels.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRegistrationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegistrationStatusId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationStatusId",
                table: "Users");
        }
    }
}
