using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Migrations
{
    /// <inheritdoc />
    public partial class adduserid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
               name: "ProductId",
               table: "Comment",
               type: "integer",
               nullable: false,
               defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Comment",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comment");
        }
    }
}
