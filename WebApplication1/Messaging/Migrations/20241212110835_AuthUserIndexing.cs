using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messaging.Migrations
{
    /// <inheritdoc />
    public partial class AuthUserIndexing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AuthUsers_Email",
                table: "AuthUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthUsers_Username",
                table: "AuthUsers",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuthUsers_Email",
                table: "AuthUsers");

            migrationBuilder.DropIndex(
                name: "IX_AuthUsers_Username",
                table: "AuthUsers");
        }
    }
}
