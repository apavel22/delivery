using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "courier_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courier_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transport",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    speed = table.Column<int>(type: "integer", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "courier",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    transport_id = table.Column<int>(type: "integer", nullable: false),
                    location_x = table.Column<int>(type: "integer", nullable: false),
                    location_y = table.Column<int>(type: "integer", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courier", x => x.id);
                    table.ForeignKey(
                        name: "FK_courier_courier_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "courier_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_courier_transport_transport_id",
                        column: x => x.transport_id,
                        principalTable: "transport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    courier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    location_x = table.Column<int>(type: "integer", nullable: false),
                    location_y = table.Column<int>(type: "integer", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_courier_courier_id",
                        column: x => x.courier_id,
                        principalTable: "courier",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_order_order_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "order_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "courier_statuses",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "notavailable" },
                    { 2, "ready" },
                    { 3, "busy" }
                });

            migrationBuilder.InsertData(
                table: "order_statuses",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "created" },
                    { 2, "assigned" },
                    { 3, "completed" }
                });

            migrationBuilder.InsertData(
                table: "transport",
                columns: new[] { "id", "name", "capacity", "speed" },
                values: new object[,]
                {
                    { 1, "pedestrian", 1, 1 },
                    { 2, "bicycle", 4, 2 },
                    { 3, "scooter", 6, 3 },
                    { 4, "car", 8, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_courier_status_id",
                table: "courier",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_courier_transport_id",
                table: "courier",
                column: "transport_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_courier_id",
                table: "order",
                column: "courier_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_id",
                table: "order",
                column: "status_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "courier");

            migrationBuilder.DropTable(
                name: "order_statuses");

            migrationBuilder.DropTable(
                name: "courier_statuses");

            migrationBuilder.DropTable(
                name: "transport");
        }
    }
}
