using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messaging.Migrations
{
    /// <inheritdoc />
    public partial class AuthUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "AuthUsers",
                newName: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Roles",
                table: "AuthUsers",
                newName: "Role");
        }
    }
}
