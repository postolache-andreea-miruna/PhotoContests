using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoContests.Migrations
{
    /// <inheritdoc />
    public partial class assignementDateReportMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "reportMessage",
                table: "Participations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "assignementDate",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reportMessage",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "assignementDate",
                table: "Assignments");
        }
    }
}
