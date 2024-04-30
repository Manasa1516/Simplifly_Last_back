using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simplifly.Migrations
{
    public partial class cancel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CancelledBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CancelledBookings_UserId",
                table: "CancelledBookings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CancelledBookings_Customers_UserId",
                table: "CancelledBookings",
                column: "UserId",
                principalTable: "Customers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CancelledBookings_Customers_UserId",
                table: "CancelledBookings");

            migrationBuilder.DropIndex(
                name: "IX_CancelledBookings_UserId",
                table: "CancelledBookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CancelledBookings");
        }
    }
}
