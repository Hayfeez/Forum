using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class RemoveHeaderImageUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaderImageUrl",
                table: "SubscriberUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeaderImageUrl",
                table: "SubscriberUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
