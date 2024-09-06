using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatetransactionentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46a9f2ed-8738-448a-9ca9-3afa00eee4ff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48732a60-c9aa-4aec-9ef3-b880ab162088");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a15c66d-6214-4dbb-a00d-10a4f7ca4cf8");

            migrationBuilder.AddColumn<long>(
                name: "TrackingId",
                table: "Transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "57d809b2-7032-45e1-b2b1-243e38e218bd", null, "DataAdmin", "DATAADMIN" },
                    { "c498039b-a975-46ce-9f43-126bcfddd220", null, "Admin", "ADMIN" },
                    { "dc5862c7-2360-4213-b0f4-db79cb883df7", null, "DataAnalyst", "DATAANALYST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57d809b2-7032-45e1-b2b1-243e38e218bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c498039b-a975-46ce-9f43-126bcfddd220");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc5862c7-2360-4213-b0f4-db79cb883df7");

            migrationBuilder.DropColumn(
                name: "TrackingId",
                table: "Transactions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "46a9f2ed-8738-448a-9ca9-3afa00eee4ff", null, "DataAdmin", "DATAADMIN" },
                    { "48732a60-c9aa-4aec-9ef3-b880ab162088", null, "DataAnalyst", "DATAANALYST" },
                    { "5a15c66d-6214-4dbb-a00d-10a4f7ca4cf8", null, "Admin", "ADMIN" }
                });
        }
    }
}
