using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.DbModels.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameOfUserColumnInSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Sessions",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                newName: "IX_Sessions_PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "Sessions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_PlayerId",
                table: "Sessions",
                newName: "IX_Sessions_UserId");
        }
    }
}
