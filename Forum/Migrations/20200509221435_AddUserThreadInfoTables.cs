using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class AddUserThreadInfoTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "ThreadReplyInfo");

            migrationBuilder.AlterColumn<int>(
                name: "Upvote",
                table: "ThreadReplyInfo",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Downvote",
                table: "ThreadReplyInfo",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "Shares",
                table: "ThreadReplyInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Viewers",
                table: "ThreadInfo",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Likes",
                table: "ThreadInfo",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Followers",
                table: "ThreadInfo",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "Flags",
                table: "ThreadInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DownvoteLimit",
                table: "Subscribers",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "UserBookmarkedThread",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadId = table.Column<long>(nullable: false),
                    SubscriberUserId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBookmarkedThread", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBookmarkedThread_SubscriberUsers_SubscriberUserId",
                        column: x => x.SubscriberUserId,
                        principalTable: "SubscriberUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBookmarkedThread_Threads_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "Threads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFlaggedThread",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadId = table.Column<long>(nullable: false),
                    SubscriberUserId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFlaggedThread", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFlaggedThread_SubscriberUsers_SubscriberUserId",
                        column: x => x.SubscriberUserId,
                        principalTable: "SubscriberUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFlaggedThread_Threads_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "Threads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFollowedPeople",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFollowingId = table.Column<long>(nullable: false),
                    UserFollowerId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    SubscriberUserId = table.Column<long>(nullable: true),
                    SubscriberUserId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowedPeople", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFollowedPeople_SubscriberUsers_SubscriberUserId",
                        column: x => x.SubscriberUserId,
                        principalTable: "SubscriberUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFollowedPeople_SubscriberUsers_SubscriberUserId1",
                        column: x => x.SubscriberUserId1,
                        principalTable: "SubscriberUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserFollowedThread",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadId = table.Column<long>(nullable: false),
                    SubscriberUserId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowedThread", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFollowedThread_SubscriberUsers_SubscriberUserId",
                        column: x => x.SubscriberUserId,
                        principalTable: "SubscriberUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFollowedThread_Threads_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "Threads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBookmarkedThread_SubscriberUserId",
                table: "UserBookmarkedThread",
                column: "SubscriberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookmarkedThread_ThreadId",
                table: "UserBookmarkedThread",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFlaggedThread_SubscriberUserId",
                table: "UserFlaggedThread",
                column: "SubscriberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFlaggedThread_ThreadId",
                table: "UserFlaggedThread",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowedPeople_SubscriberUserId",
                table: "UserFollowedPeople",
                column: "SubscriberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowedPeople_SubscriberUserId1",
                table: "UserFollowedPeople",
                column: "SubscriberUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowedThread_SubscriberUserId",
                table: "UserFollowedThread",
                column: "SubscriberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowedThread_ThreadId",
                table: "UserFollowedThread",
                column: "ThreadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBookmarkedThread");

            migrationBuilder.DropTable(
                name: "UserFlaggedThread");

            migrationBuilder.DropTable(
                name: "UserFollowedPeople");

            migrationBuilder.DropTable(
                name: "UserFollowedThread");

            migrationBuilder.DropColumn(
                name: "Shares",
                table: "ThreadReplyInfo");

            migrationBuilder.DropColumn(
                name: "Flags",
                table: "ThreadInfo");

            migrationBuilder.AlterColumn<long>(
                name: "Upvote",
                table: "ThreadReplyInfo",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "Downvote",
                table: "ThreadReplyInfo",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<long>(
                name: "Likes",
                table: "ThreadReplyInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "Viewers",
                table: "ThreadInfo",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "Likes",
                table: "ThreadInfo",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "Followers",
                table: "ThreadInfo",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "DownvoteLimit",
                table: "Subscribers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
