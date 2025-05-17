using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bipki.Database.Migrations
{
    /// <inheritdoc />
    public partial class makelocationconferencesfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "conferences_location_id_fkey",
                table: "conferences");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropIndex(
                name: "IX_conferences_location_id",
                table: "conferences");

            migrationBuilder.DropColumn(
                name: "location_id",
                table: "conferences");

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "conferences",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                column: "ConcurrencyStamp",
                value: "d9a4a3da-6fd1-422e-9f6e-27f8fda9ecac");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "location",
                table: "conferences");

            migrationBuilder.AddColumn<Guid>(
                name: "location_id",
                table: "conferences",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("locations_pkey", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                column: "ConcurrencyStamp",
                value: "efb8e06e-4764-40f3-9d02-c91a658704ca");

            migrationBuilder.CreateIndex(
                name: "IX_conferences_location_id",
                table: "conferences",
                column: "location_id");

            migrationBuilder.AddForeignKey(
                name: "conferences_location_id_fkey",
                table: "conferences",
                column: "location_id",
                principalTable: "locations",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
