using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class SubscriberInvite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Channels_ChannelId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Subscribers_SubscriberId",
                table: "Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriberUsers_Subscribers_SubscriberId",
                table: "SubscriberUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadReplies_SubscriberUsers_SubscriberUserId",
                table: "ThreadReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadReplies_Threads_ThreadId",
                table: "ThreadReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Categories_CategoryId",
                table: "Threads");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_SubscriberUsers_SubscriberUserId",
                table: "Threads");

            migrationBuilder.AlterColumn<long>(
                name: "SubscriberUserId",
                table: "Threads",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Threads",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ThreadId",
                table: "ThreadReplies",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SubscriberUserId",
                table: "ThreadReplies",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubscriberId",
                table: "SubscriberUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubscriberId",
                table: "Channels",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChannelId",
                table: "Categories",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SubscriberInvites",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    InviteCode = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    SubscriberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriberInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriberInvites_Subscribers_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Subscribers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberInvites_SubscriberId",
                table: "SubscriberInvites",
                column: "SubscriberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Channels_ChannelId",
                table: "Categories",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Subscribers_SubscriberId",
                table: "Channels",
                column: "SubscriberId",
                principalTable: "Subscribers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriberUsers_Subscribers_SubscriberId",
                table: "SubscriberUsers",
                column: "SubscriberId",
                principalTable: "Subscribers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadReplies_SubscriberUsers_SubscriberUserId",
                table: "ThreadReplies",
                column: "SubscriberUserId",
                principalTable: "SubscriberUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadReplies_Threads_ThreadId",
                table: "ThreadReplies",
                column: "ThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Categories_CategoryId",
                table: "Threads",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_SubscriberUsers_SubscriberUserId",
                table: "Threads",
                column: "SubscriberUserId",
                principalTable: "SubscriberUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Channels_ChannelId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Subscribers_SubscriberId",
                table: "Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriberUsers_Subscribers_SubscriberId",
                table: "SubscriberUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadReplies_SubscriberUsers_SubscriberUserId",
                table: "ThreadReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadReplies_Threads_ThreadId",
                table: "ThreadReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Categories_CategoryId",
                table: "Threads");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_SubscriberUsers_SubscriberUserId",
                table: "Threads");

            migrationBuilder.DropTable(
                name: "SubscriberInvites");

            migrationBuilder.AlterColumn<long>(
                name: "SubscriberUserId",
                table: "Threads",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Threads",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "ThreadId",
                table: "ThreadReplies",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "SubscriberUserId",
                table: "ThreadReplies",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "SubscriberId",
                table: "SubscriberUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "SubscriberId",
                table: "Channels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ChannelId",
                table: "Categories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Channels_ChannelId",
                table: "Categories",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Subscribers_SubscriberId",
                table: "Channels",
                column: "SubscriberId",
                principalTable: "Subscribers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriberUsers_Subscribers_SubscriberId",
                table: "SubscriberUsers",
                column: "SubscriberId",
                principalTable: "Subscribers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadReplies_SubscriberUsers_SubscriberUserId",
                table: "ThreadReplies",
                column: "SubscriberUserId",
                principalTable: "SubscriberUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadReplies_Threads_ThreadId",
                table: "ThreadReplies",
                column: "ThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Categories_CategoryId",
                table: "Threads",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_SubscriberUsers_SubscriberUserId",
                table: "Threads",
                column: "SubscriberUserId",
                principalTable: "SubscriberUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
