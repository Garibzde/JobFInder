using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFind.Migrations
{
    /// <inheritdoc />
    public partial class crttabl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NatureId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Natures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Natures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_NatureId",
                table: "Jobs",
                column: "NatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Natures_NatureId",
                table: "Jobs",
                column: "NatureId",
                principalTable: "Natures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Natures_NatureId",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "Natures");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_NatureId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "NatureId",
                table: "Jobs");
        }
    }
}
