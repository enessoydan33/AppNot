using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotUyg.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Poll",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Poll_UserId",
                table: "Poll",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Poll_AspNetUsers_UserId",
                table: "Poll",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Poll_AspNetUsers_UserId",
                table: "Poll");

            migrationBuilder.DropIndex(
                name: "IX_Poll_UserId",
                table: "Poll");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Poll");
        }
    }
}
