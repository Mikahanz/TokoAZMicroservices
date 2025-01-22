
using Microsoft.EntityFrameworkCore.Migrations;

namespace Discount.gRPC.Migrations;

public class InitialCreate : Microsoft.EntityFrameworkCore.Migrations.Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Coupons",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ProductName = table.Column<string>(type: "TEXT",nullable: false),
                Description = table.Column<string>(type: "TEXT", nullable: false),
                Amount = table.Column<int>(type: "INTEGER",nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Coupons", x => x.Id);
            });
        
        migrationBuilder.InsertData(
            table: "Coupons",
            columns: new[] { "Id", "Amount", "Description", "ProductName" },
            values: new object[,]
            {
                { 1, 150, "IPhone Discount", "IPhone X" },
                { 2, 100, "Samsung Discount", "Samsung 10" }
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Coupons");
    }
}