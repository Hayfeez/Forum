using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class AddUserFollowerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriberUsers_UserPeopleInfo_UserPeopleInfoId",
                table: "SubscriberUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_UserThreadInfo_UserThreadInfoId",
                table: "Threads");

            migrationBuilder.DropTable(
                name: "UserPeopleInfo");

            migrationBuilder.DropIndex(
                name: "IX_Threads_UserThreadInfoId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_SubscriberUsers_UserPeopleInfoId",
                table: "SubscriberUsers");

            migrationBuilder.DropColumn(
                name: "UserThreadInfoId",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "UserPeopleInfoId",
                table: "SubscriberUsers");

            migrationBuilder.CreateTable(
                name: "UserFollower",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriberUserId = table.Column<long>(nullable: false),
                    PersonId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollower", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFollower_SubscriberUsers_SubscriberUserId",
                        column: x => x.SubscriberUserId,
                        principalTable: "SubscriberUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserThreadInfo_ThreadId",
                table: "UserThreadInfo",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollower_SubscriberUserId",
                table: "UserFollower",
                column: "SubscriberUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserThreadInfo_Threads_ThreadId",
                table: "UserThreadInfo",
                column: "ThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserThreadInfo_Threads_ThreadId",
                table: "UserThreadInfo");

            migrationBuilder.DropTable(
                name: "UserFollower");

            migrationBuilder.DropIndex(
                name: "IX_UserThreadInfo_ThreadId",
                table: "UserThreadInfo");

            migrationBuilder.AddColumn<long>(
                name: "UserThreadInfoId",
                table: "Threads",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserPeopleInfoId",
                table: "SubscriberUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserPeopleInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Followed = table.Column<bool>(type: "bit", nullable: false),
                    PersonId = table.Column<long>(type: "bigint", nullable: false),
                    SubscriberUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPeopleInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPeopleInfo_SubscriberUsers_SubscriberUserId",
                        column: x => x.SubscriberUserId,
                        principalTable: "SubscriberUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Threads_UserThreadInfoId",
                table: "Threads",
                column: "UserThreadInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberUsers_UserPeopleInfoId",
                table: "SubscriberUsers",
                column: "UserPeopleInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPeopleInfo_SubscriberUserId",
                table: "UserPeopleInfo",
                column: "SubscriberUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriberUsers_UserPeopleInfo_UserPeopleInfoId",
                table: "SubscriberUsers",
                column: "UserPeopleInfoId",
                principalTable: "UserPeopleInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_UserThreadInfo_UserThreadInfoId",
                table: "Threads",
                column: "UserThreadInfoId",
                principalTable: "UserThreadInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
