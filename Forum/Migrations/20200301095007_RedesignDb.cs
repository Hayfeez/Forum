using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class RedesignDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Subscribers_SubscriberId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadReplies_AspNetUsers_AppUserId",
                table: "ThreadReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_AspNetUsers_AppUserId",
                table: "Threads");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_SubCategories_SubCategoryId",
                table: "Threads");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Threads_AppUserId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Threads_SubCategoryId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_ThreadReplies_AppUserId",
                table: "ThreadReplies");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SubscriberId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "ThreadReplies");

            migrationBuilder.DropColumn(
                name: "DateJoined",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HeaderImageUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubscriberId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Threads",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubscriberUserId",
                table: "Threads",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubscriberUserId",
                table: "ThreadReplies",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ChannelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscriberUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<double>(nullable: false),
                    HeaderImageUrl = table.Column<string>(nullable: true),
                    ProfileImageUrl = table.Column<string>(nullable: true),
                    DateJoined = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    SubscriberId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriberUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriberUsers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubscriberUsers_Subscribers_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Subscribers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Threads_CategoryId",
                table: "Threads",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_SubscriberUserId",
                table: "Threads",
                column: "SubscriberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadReplies_SubscriberUserId",
                table: "ThreadReplies",
                column: "SubscriberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ChannelId",
                table: "Categories",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberUsers_ApplicationUserId",
                table: "SubscriberUsers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberUsers_SubscriberId",
                table: "SubscriberUsers",
                column: "SubscriberId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadReplies_SubscriberUsers_SubscriberUserId",
                table: "ThreadReplies",
                column: "SubscriberUserId",
                principalTable: "SubscriberUsers",
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
                name: "FK_ThreadReplies_SubscriberUsers_SubscriberUserId",
                table: "ThreadReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Categories_CategoryId",
                table: "Threads");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_SubscriberUsers_SubscriberUserId",
                table: "Threads");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "SubscriberUsers");

            migrationBuilder.DropIndex(
                name: "IX_Threads_CategoryId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Threads_SubscriberUserId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_ThreadReplies_SubscriberUserId",
                table: "ThreadReplies");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "SubscriberUserId",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "SubscriberUserId",
                table: "ThreadReplies");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Threads",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "Threads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "ThreadReplies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateJoined",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubscriberId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Threads_AppUserId",
                table: "Threads",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_SubCategoryId",
                table: "Threads",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadReplies_AppUserId",
                table: "ThreadReplies",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SubscriberId",
                table: "AspNetUsers",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_ChannelId",
                table: "SubCategories",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Subscribers_SubscriberId",
                table: "AspNetUsers",
                column: "SubscriberId",
                principalTable: "Subscribers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadReplies_AspNetUsers_AppUserId",
                table: "ThreadReplies",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_AspNetUsers_AppUserId",
                table: "Threads",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_SubCategories_SubCategoryId",
                table: "Threads",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
