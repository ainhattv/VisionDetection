using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlobService.Data.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlobContainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    WorkPlaceId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlobContainers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlobFolders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    BlobContainerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlobFolders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlobFolders_BlobContainers_BlobContainerId",
                        column: x => x.BlobContainerId,
                        principalTable: "BlobContainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlobFolders_BlobContainerId",
                table: "BlobFolders",
                column: "BlobContainerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlobFolders");

            migrationBuilder.DropTable(
                name: "BlobContainers");
        }
    }
}
