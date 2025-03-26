using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curotec.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class story002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "TodoLists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[] { new Guid("718fd5e2-31d4-4da6-bb29-a42304d22197"), "johndoe@example.com", "John Doe", "hashed_password" });

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_UserId",
                table: "TodoLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLists_Users_UserId",
                table: "TodoLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoLists_Users_UserId",
                table: "TodoLists");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TodoLists_UserId",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TodoLists");
        }
    }
}
