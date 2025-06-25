using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NET_Core_Car_Rental_System_Project_F.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDatabaseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModel_CarBrand_CarBrandId",
                table: "CarModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModel_CarModelId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarModel",
                table: "CarModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarBrand",
                table: "CarBrand");

            migrationBuilder.RenameTable(
                name: "CarModel",
                newName: "CarModels");

            migrationBuilder.RenameTable(
                name: "CarBrand",
                newName: "CarBrands");

            migrationBuilder.RenameIndex(
                name: "IX_CarModel_CarBrandId",
                table: "CarModels",
                newName: "IX_CarModels_CarBrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarModels",
                table: "CarModels",
                column: "CarModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarBrands",
                table: "CarBrands",
                column: "CarBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarBrands_CarBrandId",
                table: "CarModels",
                column: "CarBrandId",
                principalTable: "CarBrands",
                principalColumn: "CarBrandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarModels_CarModelId",
                table: "Cars",
                column: "CarModelId",
                principalTable: "CarModels",
                principalColumn: "CarModelId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarBrands_CarBrandId",
                table: "CarModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModels_CarModelId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarModels",
                table: "CarModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarBrands",
                table: "CarBrands");

            migrationBuilder.RenameTable(
                name: "CarModels",
                newName: "CarModel");

            migrationBuilder.RenameTable(
                name: "CarBrands",
                newName: "CarBrand");

            migrationBuilder.RenameIndex(
                name: "IX_CarModels_CarBrandId",
                table: "CarModel",
                newName: "IX_CarModel_CarBrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarModel",
                table: "CarModel",
                column: "CarModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarBrand",
                table: "CarBrand",
                column: "CarBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModel_CarBrand_CarBrandId",
                table: "CarModel",
                column: "CarBrandId",
                principalTable: "CarBrand",
                principalColumn: "CarBrandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarModel_CarModelId",
                table: "Cars",
                column: "CarModelId",
                principalTable: "CarModel",
                principalColumn: "CarModelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
