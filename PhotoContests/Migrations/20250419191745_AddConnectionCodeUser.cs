using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoContests.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionCodeUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "connectionCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "connectionCode",
                table: "AspNetUsers");
        }
    }
}
