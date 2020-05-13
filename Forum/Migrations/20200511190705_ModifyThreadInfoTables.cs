using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class ModifyThreadInfoTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Flags",
                table: "ThreadInfo");

            migrationBuilder.DropColumn(
                name: "Followers",
                table: "ThreadInfo");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "ThreadInfo");

            migrationBuilder.DropColumn(
                name: "Viewers",
                table: "ThreadInfo");

            migrationBuilder.AddColumn<long>(
                name: "UserThreadInfoId",
                table: "Threads",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "ThreadInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "UserPeopleInfoId",
                table: "SubscriberUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserPeopleInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<long>(nullable: false),
                    SubscriberUserId = table.Column<long>(nullable: false),
                    Followed = table.Column<bool>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "UserThreadInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadId = table.Column<long>(nullable: false),
                    SubscriberUserId = table.Column<long>(nullable: false),
                    Flagged = table.Column<bool>(nullable: false),
                    Bookmarked = table.Column<bool>(nullable: false),
                    Followed = table.Column<bool>(nullable: false),
                    Liked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserThreadInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserThreadInfo_SubscriberUsers_SubscriberUserId",
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

            migrationBuilder.CreateIndex(
                name: "IX_UserThreadInfo_SubscriberUserId",
                table: "UserThreadInfo",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriberUsers_UserPeopleInfo_UserPeopleInfoId",
                table: "SubscriberUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_UserThreadInfo_UserThreadInfoId",
                table: "Threads");

            migrationBuilder.DropTable(
                name: "UserPeopleInfo");

            migrationBuilder.DropTable(
                name: "UserThreadInfo");

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
                name: "Views",
                table: "ThreadInfo");

            migrationBuilder.DropColumn(
                name: "UserPeopleInfoId",
                table: "SubscriberUsers");

            migrationBuilder.AddColumn<int>(
                name: "Flags",
                table: "ThreadInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Followers",
                table: "ThreadInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "ThreadInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Viewers",
                table: "ThreadInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserBookmarkedThread",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscriberUserId = table.Column<long>(type: "bigint", nullable: false),
                    ThreadId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscriberUserId = table.Column<long>(type: "bigint", nullable: false),
                    ThreadId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscriberUserId = table.Column<long>(type: "bigint", nullable: true),
                    SubscriberUserId1 = table.Column<long>(type: "bigint", nullable: true),
                    UserFollowerId = table.Column<long>(type: "bigint", nullable: false),
                    UserFollowingId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscriberUserId = table.Column<long>(type: "bigint", nullable: false),
                    ThreadId = table.Column<long>(type: "bigint", nullable: false)
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
    }
}
