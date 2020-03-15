using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class UpdateSubscriberUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "SubscriberUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "SubscriberUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "SubscriberUsers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "SubscriberUsers");
        }
    }
}
