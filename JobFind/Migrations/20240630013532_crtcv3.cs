﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFind.Migrations
{
    /// <inheritdoc />
    public partial class crtcv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniversityName",
                table: "CVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "İnformationAboutYourself",
                table: "CVs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniversityName",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "İnformationAboutYourself",
                table: "CVs");
        }
    }
}
