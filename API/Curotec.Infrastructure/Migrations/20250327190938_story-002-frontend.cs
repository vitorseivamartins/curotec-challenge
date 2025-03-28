using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curotec.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class story002frontend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("718fd5e2-31d4-4da6-bb29-a42304d22197"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash" },
                values: new object[] { new Guid("a939e1dd-811e-4405-97e2-b2be982c0e9a"), "johndoe@example.com", "$2a$11$bDMp5eKT9zSKvW0sRGfShOF3sWyDdVGy0.BhnCNogQXxFQApnPqk6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a939e1dd-811e-4405-97e2-b2be982c0e9a"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[] { new Guid("718fd5e2-31d4-4da6-bb29-a42304d22197"), "johndoe@example.com", "John Doe", "hashed_password" });
        }
    }
}
