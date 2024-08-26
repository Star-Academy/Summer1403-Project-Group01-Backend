using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nodesedges : Migration
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
                name: "NodeType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NodeAttribute",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: false),
                    NodeTypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeAttribute_NodeType_NodeTypeId",
                        column: x => x.NodeTypeId,
                        principalTable: "NodeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_Nodes_NodeType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "NodeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EdgeTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: false),
                    SourceNodeAttributeId = table.Column<long>(type: "bigint", nullable: false),
                    DestinationNodeAttributeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdgeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EdgeTypes_NodeAttribute_DestinationNodeAttributeId",
                        column: x => x.DestinationNodeAttributeId,
                        principalTable: "NodeAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdgeTypes_NodeAttribute_SourceNodeAttributeId",
                        column: x => x.SourceNodeAttributeId,
                        principalTable: "NodeAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NodeAttributeValue",
                columns: table => new
                {
                    NodeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NodeId1 = table.Column<long>(type: "bigint", nullable: false),
                    NodeAttributeId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeAttributeValue", x => x.NodeId);
                    table.ForeignKey(
                        name: "FK_NodeAttributeValue_NodeAttribute_NodeAttributeId",
                        column: x => x.NodeAttributeId,
                        principalTable: "NodeAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NodeAttributeValue_Nodes_NodeId1",
                        column: x => x.NodeId1,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EdgeAttributes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: false),
                    EdgeTypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdgeAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EdgeAttributes_EdgeTypes_EdgeTypeId",
                        column: x => x.EdgeTypeId,
                        principalTable: "EdgeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Edges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SourceValue = table.Column<long>(type: "bigint", nullable: false),
                    DestinationValue = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Edges_EdgeTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "EdgeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Edges_Nodes_DestinationValue",
                        column: x => x.DestinationValue,
                        principalTable: "Nodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Edges_Nodes_SourceValue",
                        column: x => x.SourceValue,
                        principalTable: "Nodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EdgeAttributeValues",
                columns: table => new
                {
                    EdgeId = table.Column<long>(type: "bigint", nullable: false),
                    EdgeAttributeId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdgeAttributeValues", x => new { x.EdgeId, x.EdgeAttributeId });
                    table.ForeignKey(
                        name: "FK_EdgeAttributeValues_EdgeAttributes_EdgeAttributeId",
                        column: x => x.EdgeAttributeId,
                        principalTable: "EdgeAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdgeAttributeValues_Edges_EdgeId",
                        column: x => x.EdgeId,
                        principalTable: "Edges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "092f423e-8016-45cf-af2c-bcbf65b7c012", null, "DataAdmin", "DATAADMIN" },
                    { "1e432022-2e6a-43f7-abc9-e178e6438756", null, "Admin", "ADMIN" },
                    { "3afa9c92-0802-4ecf-a7e9-e053796a4c01", null, "DataAnalyst", "DATAANALYST" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EdgeAttributes_EdgeTypeId",
                table: "EdgeAttributes",
                column: "EdgeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EdgeAttributeValues_EdgeAttributeId",
                table: "EdgeAttributeValues",
                column: "EdgeAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Edges_DestinationValue",
                table: "Edges",
                column: "DestinationValue");

            migrationBuilder.CreateIndex(
                name: "IX_Edges_SourceValue",
                table: "Edges",
                column: "SourceValue");

            migrationBuilder.CreateIndex(
                name: "IX_Edges_TypeId",
                table: "Edges",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EdgeTypes_DestinationNodeAttributeId",
                table: "EdgeTypes",
                column: "DestinationNodeAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_EdgeTypes_SourceNodeAttributeId",
                table: "EdgeTypes",
                column: "SourceNodeAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeAttribute_NodeTypeId",
                table: "NodeAttribute",
                column: "NodeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeAttributeValue_NodeAttributeId",
                table: "NodeAttributeValue",
                column: "NodeAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeAttributeValue_NodeId1",
                table: "NodeAttributeValue",
                column: "NodeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_TypeId",
                table: "Nodes",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EdgeAttributeValues");

            migrationBuilder.DropTable(
                name: "NodeAttributeValue");

            migrationBuilder.DropTable(
                name: "EdgeAttributes");

            migrationBuilder.DropTable(
                name: "Edges");

            migrationBuilder.DropTable(
                name: "EdgeTypes");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "NodeAttribute");

            migrationBuilder.DropTable(
                name: "NodeType");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "092f423e-8016-45cf-af2c-bcbf65b7c012");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e432022-2e6a-43f7-abc9-e178e6438756");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3afa9c92-0802-4ecf-a7e9-e053796a4c01");

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
