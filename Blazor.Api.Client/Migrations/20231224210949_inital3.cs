using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Api.Client.Migrations
{
    /// <inheritdoc />
    public partial class inital3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LasName",
                table: "Client",
                newName: "LastName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Client",
                newName: "LasName");
        }
    }
}
