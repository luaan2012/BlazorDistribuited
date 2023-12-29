using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Api.Client.Migrations
{
    /// <inheritdoc />
    public partial class inital4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Client");
        }
    }
}
