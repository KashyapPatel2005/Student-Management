using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDMVC.Migrations
{
    /// <inheritdoc />
    public partial class TestModuleAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TestAttempts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempts_UserId",
                table: "TestAttempts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestAttempts_AspNetUsers_UserId",
                table: "TestAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestAttempts_AspNetUsers_UserId",
                table: "TestAttempts");

            migrationBuilder.DropIndex(
                name: "IX_TestAttempts_UserId",
                table: "TestAttempts");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TestAttempts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
