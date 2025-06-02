using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoContests.Migrations
{
    /// <inheritdoc />
    public partial class Videotable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_AspNetUsers_idJuror",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_AspNetUsers_idPhotographer",
                table: "Videos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Videos",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_idJuror",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "name",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "idPhotographer",
                table: "Videos");

            /*migrationBuilder.RenameColumn(
                name: "idPhotographer",
                table: "Videos",
                newName: "JurorId");*/

            migrationBuilder.AlterColumn<string>(
                name: "idJuror",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "idVideo",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "idCompetition",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "idPhoto",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "idSection",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Videos",
                table: "Videos",
                column: "idVideo");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_idPhoto_idCompetition_idSection",
                table: "Videos",
                columns: new[] { "idPhoto", "idCompetition", "idSection" },
                unique: true);

            /*migrationBuilder.CreateIndex(
                name: "IX_Videos_JurorId",
                table: "Videos",
                column: "JurorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_AspNetUsers_JurorId",
                table: "Videos",
                column: "JurorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);*/
            migrationBuilder.CreateIndex(
                name: "IX_Videos_idJuror",
                table: "Videos",
                column: "idJuror");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_AspNetUsers_idJuror",
                table: "Videos",
                column: "idJuror",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Participations_idPhoto_idCompetition_idSection",
                table: "Videos",
                columns: new[] { "idPhoto", "idCompetition", "idSection" },
                principalTable: "Participations",
                principalColumns: new[] { "idPhoto", "idCompetition", "idSection" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_Videos_AspNetUsers_JurorId",
                table: "Videos");*/
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_AspNetUsers_idJuror",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Participations_idPhoto_idCompetition_idSection",
                table: "Videos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Videos",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_idPhoto_idCompetition_idSection",
                table: "Videos");

            /*migrationBuilder.DropIndex(
                name: "IX_Videos_JurorId",
                table: "Videos");*/
            migrationBuilder.DropIndex(
                name: "IX_Videos_idJuror",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "idVideo",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "idCompetition",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "idPhoto",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "idSection",
                table: "Videos");

            /*migrationBuilder.RenameColumn(
                name: "JurorId",
                table: "Videos",
                newName: "idPhotographer");*/
 
            migrationBuilder.AlterColumn<string>(
                name: "idJuror",
                table: "Videos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Videos",
                table: "Videos",
                columns: new[] { "idPhotographer", "idJuror", "date" });

            migrationBuilder.CreateIndex(
                name: "IX_Videos_idJuror",
                table: "Videos",
                column: "idJuror");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_AspNetUsers_idJuror",
                table: "Videos",
                column: "idJuror",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_AspNetUsers_idPhotographer",
                table: "Videos",
                column: "idPhotographer",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
