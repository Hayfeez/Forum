using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class AddUserReplyInfoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserThreadReplyInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriberUserId = table.Column<long>(nullable: false),
                    ThreadReplyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserThreadReplyInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserThreadReplyInfo_SubscriberUsers_SubscriberUserId",
                        column: x => x.SubscriberUserId,
                        principalTable: "SubscriberUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserThreadReplyInfo_ThreadReplies_ThreadReplyId",
                        column: x => x.ThreadReplyId,
                        principalTable: "ThreadReplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserThreadReplyInfo_SubscriberUserId",
                table: "UserThreadReplyInfo",
                column: "SubscriberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserThreadReplyInfo_ThreadReplyId",
                table: "UserThreadReplyInfo",
                column: "ThreadReplyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserThreadReplyInfo");
        }
    }
}
