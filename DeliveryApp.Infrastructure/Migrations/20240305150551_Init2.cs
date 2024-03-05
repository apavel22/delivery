using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "courier_statuses",
                keyColumn: "id",
                keyValue: 3,
                column: "name",
                value: "inwork");

            migrationBuilder.UpdateData(
                table: "order_statuses",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "new");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "courier_statuses",
                keyColumn: "id",
                keyValue: 3,
                column: "name",
                value: "busy");

            migrationBuilder.UpdateData(
                table: "order_statuses",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "created");
        }
    }
}
