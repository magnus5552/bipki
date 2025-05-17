using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bipki.Database.Migrations
{
    /// <inheritdoc />
    public partial class addadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "conference_id",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<bool>(
                name: "NotificationEnabled",
                table: "activity_registrations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalParticipants",
                table: "activities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "conference_id", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "surname", "telegram", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"), 0, "e597ae36-25d3-4bb3-820f-e03359c7a621", null, null, false, false, null, "admin", null, "ADMIN", null, null, false, null, "admin", "adminTg", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("5e2b6f6f-a877-46ca-9b82-cc7d6a4118d5"), new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("5e2b6f6f-a877-46ca-9b82-cc7d6a4118d5"), new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a") });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"));

            migrationBuilder.DropColumn(
                name: "NotificationEnabled",
                table: "activity_registrations");

            migrationBuilder.DropColumn(
                name: "TotalParticipants",
                table: "activities");

            migrationBuilder.AlterColumn<Guid>(
                name: "conference_id",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
