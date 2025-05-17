using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bipki.Database.Migrations
{
    /// <inheritdoc />
    public partial class addadmintouserrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("4bcb87c6-3320-4de0-8e7c-c6765a08916b"), new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a") });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                column: "ConcurrencyStamp",
                value: "efb8e06e-4764-40f3-9d02-c91a658704ca");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("4bcb87c6-3320-4de0-8e7c-c6765a08916b"), new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a") });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                column: "ConcurrencyStamp",
                value: "e597ae36-25d3-4bb3-820f-e03359c7a621");
        }
    }
}
