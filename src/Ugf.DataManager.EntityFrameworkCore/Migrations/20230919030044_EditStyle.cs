using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ugf.DataManager.Migrations
{
    /// <inheritdoc />
    public partial class EditStyle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Style",
                table: "ClassProperties",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Style",
                table: "ClassProperties");
        }
    }
}
