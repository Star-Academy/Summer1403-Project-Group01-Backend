using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NormalizedName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "1db5e3c3-d967-4403-b64c-f3dab04f3b9c", null, "DataAnalyst", "DATAANALYST" },
                    { "6399bc6d-8c5a-4d79-9045-820446775acd", null, "DataAdmin", "DATAADMIN" },
                    { "c03dacd0-7454-42e4-b617-eeb9e3863c0d", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1db5e3c3-d967-4403-b64c-f3dab04f3b9c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6399bc6d-8c5a-4d79-9045-820446775acd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c03dacd0-7454-42e4-b617-eeb9e3863c0d");

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
    }
}
