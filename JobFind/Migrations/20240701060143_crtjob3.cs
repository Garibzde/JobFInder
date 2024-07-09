using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFind.Migrations
{
    /// <inheritdoc />
    public partial class crtjob3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "EmploymentType",
                table: "Jobs",
                newName: "CompanyName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Jobs",
                newName: "EmploymentType");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
