using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPS.Data.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkPlaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    AuthorId = table.Column<string>(nullable: false),
                    AuthorEmail = table.Column<string>(nullable: false),
                    AuthorName = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPlaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkPlaceSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    WorkPlaceId = table.Column<Guid>(nullable: false),
                    WorkPlaceSize = table.Column<int>(nullable: false),
                    WorkPlaceLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPlaceSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPlaceSettings_WorkPlaces_WorkPlaceId",
                        column: x => x.WorkPlaceId,
                        principalTable: "WorkPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaceSettings_WorkPlaceId",
                table: "WorkPlaceSettings",
                column: "WorkPlaceId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkPlaceSettings");

            migrationBuilder.DropTable(
                name: "WorkPlaces");
        }
    }
}
