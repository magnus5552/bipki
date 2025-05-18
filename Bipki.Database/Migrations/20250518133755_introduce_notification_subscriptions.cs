using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bipki.Database.Migrations
{
    /// <inheritdoc />
    public partial class introduce_notification_subscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notification_subscriptions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    endpoint = table.Column<string>(type: "text", nullable: false),
                    p256dh = table.Column<string>(type: "text", nullable: false),
                    auth = table.Column<string>(type: "text", nullable: false),
                    subscribent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "notification_subscription_subscribent_id_fkey",
                        column: x => x.subscribent_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                column: "ConcurrencyStamp",
                value: "a682055c-9897-46d3-86be-0d84094260ff");

            migrationBuilder.CreateIndex(
                name: "IX_notification_subscriptions_subscribent_id",
                table: "notification_subscriptions",
                column: "subscribent_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification_subscriptions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                column: "ConcurrencyStamp",
                value: "d5f156dc-4085-42d9-8d7c-6bcad864c174");
        }
    }
}
