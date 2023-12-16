using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarWashAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bills_PaymentMethodId",
                table: "Bills",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_VehicleTypeId",
                table: "Bills",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_WasherId",
                table: "Bills",
                column: "WasherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_PaymentMethods_PaymentMethodId",
                table: "Bills",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Vehicles_VehicleTypeId",
                table: "Bills",
                column: "VehicleTypeId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Washers_WasherId",
                table: "Bills",
                column: "WasherId",
                principalTable: "Washers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_PaymentMethods_PaymentMethodId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Vehicles_VehicleTypeId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Washers_WasherId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_PaymentMethodId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_VehicleTypeId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_WasherId",
                table: "Bills");
        }
    }
}
