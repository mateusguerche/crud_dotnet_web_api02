using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Projeto02.Migrations
{
    /// <inheritdoc />
    public partial class seedcategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO categories (Name, ImageUrl, CreatedAt, UpdatedAt) VALUES ('Bebidas', 'bebidas.jpg', NOW(), NOW());");
            migrationBuilder.Sql("INSERT INTO categories (Name, ImageUrl, CreatedAt, UpdatedAt) VALUES ('Lanches', 'lanches.jpg', NOW(), NOW());");
            migrationBuilder.Sql("INSERT INTO categories (Name, ImageUrl, CreatedAt, UpdatedAt) VALUES ('Sobremesas', 'sobremesas.jpg', NOW(), NOW());");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from categories");
        }
    }
}
