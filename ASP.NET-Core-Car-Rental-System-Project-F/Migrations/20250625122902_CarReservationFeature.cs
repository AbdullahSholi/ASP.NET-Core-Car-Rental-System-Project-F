using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NET_Core_Car_Rental_System_Project_F.Migrations
{
    /// <inheritdoc />
    public partial class CarReservationFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Cars_CarId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Users_UserId",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation");

            migrationBuilder.RenameTable(
                name: "Reservation",
                newName: "Reservations");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_UserId",
                table: "Reservations",
                newName: "IX_Reservations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_CarId",
                table: "Reservations",
                newName: "IX_Reservations_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Cars_CarId",
                table: "Reservations",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Cars_CarId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_UserId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.RenameTable(
                name: "Reservations",
                newName: "Reservation");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_UserId",
                table: "Reservation",
                newName: "IX_Reservation_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_CarId",
                table: "Reservation",
                newName: "IX_Reservation_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Cars_CarId",
                table: "Reservation",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Users_UserId",
                table: "Reservation",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
