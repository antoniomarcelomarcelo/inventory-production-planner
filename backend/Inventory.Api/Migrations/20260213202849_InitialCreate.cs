using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inventory.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "raw_materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    stock_quantity = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raw_materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bill_of_material_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    raw_material_id = table.Column<int>(type: "integer", nullable: false),
                    quantity_per_unit = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_of_material_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bill_of_material_items_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bill_of_material_items_raw_materials_raw_material_id",
                        column: x => x.raw_material_id,
                        principalTable: "raw_materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bill_of_material_items_product_id_raw_material_id",
                table: "bill_of_material_items",
                columns: new[] { "product_id", "raw_material_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_of_material_items_raw_material_id",
                table: "bill_of_material_items",
                column: "raw_material_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_code",
                table: "products",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_raw_materials_code",
                table: "raw_materials",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bill_of_material_items");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "raw_materials");
        }
    }
}
