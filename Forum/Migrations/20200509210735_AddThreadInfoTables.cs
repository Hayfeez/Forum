using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class AddThreadInfoTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Channels");

            migrationBuilder.AddColumn<int>(
                name: "DownvoteLimit",
                table: "Subscribers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlagLimit",
                table: "Subscribers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PinnedPosts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ThreadHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThreadHistory_Threads_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "Threads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThreadInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadId = table.Column<long>(nullable: false),
                    Followers = table.Column<long>(nullable: false),
                    Viewers = table.Column<long>(nullable: false),
                    Likes = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThreadInfo_Threads_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "Threads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThreadReplyInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadReplyId = table.Column<long>(nullable: false),
                    Upvote = table.Column<long>(nullable: false),
                    Downvote = table.Column<long>(nullable: false),
                    Likes = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadReplyInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThreadReplyInfo_ThreadReplies_ThreadReplyId",
                        column: x => x.ThreadReplyId,
                        principalTable: "ThreadReplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThreadHistory_ThreadId",
                table: "ThreadHistory",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadInfo_ThreadId",
                table: "ThreadInfo",
                column: "ThreadId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThreadReplyInfo_ThreadReplyId",
                table: "ThreadReplyInfo",
                column: "ThreadReplyId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThreadHistory");

            migrationBuilder.DropTable(
                name: "ThreadInfo");

            migrationBuilder.DropTable(
                name: "ThreadReplyInfo");

            migrationBuilder.DropColumn(
                name: "DownvoteLimit",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "FlagLimit",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PinnedPosts");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Channels",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
