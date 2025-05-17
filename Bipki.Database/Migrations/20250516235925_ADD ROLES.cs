using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bipki.Database.Migrations
{
    /// <inheritdoc />
    public partial class ADDROLES : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("4bcb87c6-3320-4de0-8e7c-c6765a08916b"), null, null, "USER" },
                    { new Guid("5e2b6f6f-a877-46ca-9b82-cc7d6a4118d5"), null, null, "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4bcb87c6-3320-4de0-8e7c-c6765a08916b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5e2b6f6f-a877-46ca-9b82-cc7d6a4118d5"));
        }
    }
}
