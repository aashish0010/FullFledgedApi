using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullFledgedInfrastructure.Migrations
{
    public partial class userregister12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "userRegisters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "userRegisters",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
