using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class S : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SubscriberUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SubscriberUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
