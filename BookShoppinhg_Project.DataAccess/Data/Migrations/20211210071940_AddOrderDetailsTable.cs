using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShoppinhg_Project.DataAccess.Migrations
{
    public partial class AddOrderDetailsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderHeaderId = table.Column<int>(name: "OrderHeader Id", type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductId0 = table.Column<int>(name: "Product Id", type: "int", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_OrderHeaders_OrderHeader Id",
                        column: x => x.OrderHeaderId,
                        principalTable: "OrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_products_Product Id",
                        column: x => x.ProductId0,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderHeader Id",
                table: "OrderDetails",
                column: "OrderHeader Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_Product Id",
                table: "OrderDetails",
                column: "Product Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");
        }
    }
}
