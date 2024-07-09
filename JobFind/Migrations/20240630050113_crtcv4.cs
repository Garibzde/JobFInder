using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFind.Migrations
{
    /// <inheritdoc />
    public partial class crtcv4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Skills",
                table: "CVs",
                newName: "Skills3");

            migrationBuilder.AddColumn<string>(
                name: "Skills1",
                table: "CVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Skills2",
                table: "CVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Skills1",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "Skills2",
                table: "CVs");

            migrationBuilder.RenameColumn(
                name: "Skills3",
                table: "CVs",
                newName: "Skills");
        }
    }
}
