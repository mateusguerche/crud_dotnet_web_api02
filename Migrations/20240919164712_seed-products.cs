using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Projeto02.Migrations
{
    /// <inheritdoc />
    public partial class seedproducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO products (CategoryId, Name, Description, Price, ImageUrl, Stock, CreatedAt, UpdatedAt) " +
                "VALUES (1, 'Coca-Cola Diet', 'Refrigerante de Cola 350 ml', 5.45, 'cocacola.jpg', 50, NOW(), NOW());");
           
            migrationBuilder.Sql("INSERT INTO products (CategoryId, Name, Description, Price, ImageUrl, Stock, CreatedAt, UpdatedAt) " +
                "VALUES (2, 'Lance de Atum', 'Lanche de Atum com maionese', 8.50, 'atum.jpg', 10, NOW(), NOW());");

            migrationBuilder.Sql("INSERT INTO products (CategoryId, Name, Description, Price, ImageUrl, Stock, CreatedAt, UpdatedAt) " +
                "VALUES (3, 'Pudim 100 g', 'Pudim de leite condensado 100 g', 6.75, 'pudim.jpg', 20, NOW(), NOW());");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from products");

        }
    }
}
