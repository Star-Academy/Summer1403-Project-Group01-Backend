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
                keyValue: "1db5e3c3-d967-4403-b64c-f3dab04f3b9c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6399bc6d-8c5a-4d79-9045-820446775acd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c03dacd0-7454-42e4-b617-eeb9e3863c0d");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

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
    }
}
