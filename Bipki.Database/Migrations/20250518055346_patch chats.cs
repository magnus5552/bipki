using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bipki.Database.Migrations
{
    /// <inheritdoc />
    public partial class patchchats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "sender_name",
                table: "messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ChatId",
                table: "conferences",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "chat_id",
                table: "activities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                column: "ConcurrencyStamp",
                value: "416b7295-9bb5-4908-be0f-6f7db6d0617d");

            migrationBuilder.CreateIndex(
                name: "IX_conferences_ChatId",
                table: "conferences",
                column: "ChatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_activities_chat_id",
                table: "activities",
                column: "chat_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "activities_chat_id_fkey",
                table: "activities",
                column: "chat_id",
                principalTable: "chat",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "activities_chat_id_fkey",
                table: "conferences",
                column: "ChatId",
                principalTable: "chat",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "activities_chat_id_fkey",
                table: "activities");

            migrationBuilder.DropForeignKey(
                name: "activities_chat_id_fkey",
                table: "conferences");

            migrationBuilder.DropIndex(
                name: "IX_conferences_ChatId",
                table: "conferences");

            migrationBuilder.DropIndex(
                name: "IX_activities_chat_id",
                table: "activities");

            migrationBuilder.DropColumn(
                name: "sender_name",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "conferences");

            migrationBuilder.DropColumn(
                name: "chat_id",
                table: "activities");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                column: "ConcurrencyStamp",
                value: "37fb796d-5739-427c-b7a1-69ee2be38d27");
        }
    }
}
