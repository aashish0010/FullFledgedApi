using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullFledgedInfrastructure.Migrations
{
    public partial class userregister123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "userRegisters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "userRegisters");
        }
    }
}
