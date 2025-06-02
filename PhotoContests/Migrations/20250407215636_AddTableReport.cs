using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoContests.Migrations
{
    /// <inheritdoc />
    public partial class AddTableReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idJuror",
                table: "Videos");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    idReport = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idPhoto = table.Column<int>(type: "int", nullable: false),
                    idCompetition = table.Column<int>(type: "int", nullable: false),
                    idSection = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    noAccept = table.Column<int>(type: "int", nullable: false),
                    noDecline = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.idReport);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_idUser",
                        column: x => x.idUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Participations_idPhoto_idCompetition_idSection",
                        columns: x => new { x.idPhoto, x.idCompetition, x.idSection },
                        principalTable: "Participations",
                        principalColumns: new[] { "idPhoto", "idCompetition", "idSection" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_idPhoto_idCompetition_idSection",
                table: "Reports",
                columns: new[] { "idPhoto", "idCompetition", "idSection" });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_idUser",
                table: "Reports",
                column: "idUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "idJuror",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
