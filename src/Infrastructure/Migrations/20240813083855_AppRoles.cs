using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AppRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10ed68de-3930-48bd-98e0-bc80b59ee93a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "813f20f6-6916-4930-980b-694b396d6a8c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6a084367-d2b5-4014-bbcd-7f0a8d14a7de", null, "DataAnalyst", null },
                    { "91b16f58-93ec-4ab4-b83a-2cb20580c540", null, "DataAdmin", null },
                    { "940eea0a-aff9-497e-b067-d163f7d3ac79", null, "Admin", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a084367-d2b5-4014-bbcd-7f0a8d14a7de");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "91b16f58-93ec-4ab4-b83a-2cb20580c540");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "940eea0a-aff9-497e-b067-d163f7d3ac79");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "10ed68de-3930-48bd-98e0-bc80b59ee93a", null, "Admin", "ADMIN" },
                    { "813f20f6-6916-4930-980b-694b396d6a8c", null, "User", "USER" }
                });
        }
    }
}
