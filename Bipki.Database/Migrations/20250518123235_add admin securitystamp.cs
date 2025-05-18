using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bipki.Database.Migrations
{
    /// <inheritdoc />
    public partial class addadminsecuritystamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ec888857-3fbc-4939-bab2-c127a2bc59a2", "oikamsda" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d5f156dc-4085-42d9-8d7c-6bcad864c174", null });
        }
    }
}
