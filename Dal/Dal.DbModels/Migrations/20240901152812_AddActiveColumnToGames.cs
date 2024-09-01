using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.DbModels.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveColumnToGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Games",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Games");
        }
    }
}
