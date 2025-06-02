using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoContests.Migrations
{
    /// <inheritdoc />
    public partial class Exifinfophoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cameraModel",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "exposureTime",
                table: "Photos",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "fStop",
                table: "Photos",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "isoSpeed",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "takenDate",
                table: "Photos",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cameraModel",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "exposureTime",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "fStop",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "isoSpeed",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "takenDate",
                table: "Photos");
        }
    }
}
