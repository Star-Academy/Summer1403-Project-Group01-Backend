using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CompleteUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "006c654c-771f-4591-bb9a-e7bb3443afa4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e30912b-d822-4d34-b6a0-45d7f081e7b0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cde285b6-01c1-4e35-8cb5-65870c533091");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2108a42f-ccf7-4824-baf9-fec90f53f79f", null, "Admin", "ADMIN" },
                    { "6f4f50ee-ec2b-4844-a7bd-98a56d8755e4", null, "DataAdmin", "DATAADMIN" },
                    { "cd35b7f2-76ef-4a13-9265-cf267b1dbbdb", null, "DataAnalyst", "DATAANALYST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2108a42f-ccf7-4824-baf9-fec90f53f79f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6f4f50ee-ec2b-4844-a7bd-98a56d8755e4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd35b7f2-76ef-4a13-9265-cf267b1dbbdb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "006c654c-771f-4591-bb9a-e7bb3443afa4", null, "Admin", "ADMIN" },
                    { "4e30912b-d822-4d34-b6a0-45d7f081e7b0", null, "DataAdmin", "DATAADMIN" },
                    { "cde285b6-01c1-4e35-8cb5-65870c533091", null, "DataAnalyst", "DATAANALYST" }
                });
        }
    }
}
