using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Nodes : Migration
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

            migrationBuilder.CreateTable(
                name: "NodeTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NodeAttributes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    NodeTypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeAttributes_NodeTypes_NodeTypeId",
                        column: x => x.NodeTypeId,
                        principalTable: "NodeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nodes_NodeTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "NodeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NodeAttributeValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NodeId = table.Column<long>(type: "bigint", nullable: false),
                    NodeAttributeId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeAttributeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeAttributeValues_NodeAttributes_NodeAttributeId",
                        column: x => x.NodeAttributeId,
                        principalTable: "NodeAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NodeAttributeValues_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8f2a0984-d380-4f96-86c2-28d2bbeb08ea", null, "Admin", "ADMIN" },
                    { "b9f288d2-e128-4914-b46e-c1f92b74a15b", null, "DataAdmin", "DATAADMIN" },
                    { "d35f5094-bbb2-4909-8116-4d226f5d3b69", null, "DataAnalyst", "DATAANALYST" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NodeAttributes_NodeTypeId",
                table: "NodeAttributes",
                column: "NodeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeAttributeValues_NodeAttributeId",
                table: "NodeAttributeValues",
                column: "NodeAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeAttributeValues_NodeId",
                table: "NodeAttributeValues",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_TypeId",
                table: "Nodes",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NodeAttributeValues");

            migrationBuilder.DropTable(
                name: "NodeAttributes");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "NodeTypes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f2a0984-d380-4f96-86c2-28d2bbeb08ea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9f288d2-e128-4914-b46e-c1f92b74a15b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d35f5094-bbb2-4909-8116-4d226f5d3b69");

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
