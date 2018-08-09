using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GeekBurger.Users.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "gbu");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "gbu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(nullable: false),
                    PersistedId = table.Column<Guid>(nullable: false),
                    FaceBase64 = table.Column<string>(nullable: true),
                    GuidReference = table.Column<string>(nullable: true),
                    InProcessing = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRestrictions",
                schema: "gbu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: false),
                    Ingredient = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRestrictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRestrictions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "gbu",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRestrictions_UserId",
                schema: "gbu",
                table: "UserRestrictions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRestrictions",
                schema: "gbu");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "gbu");
        }
    }
}
