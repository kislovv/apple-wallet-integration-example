using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Transfer_PushToken_In_Device_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "push_token",
                table: "apple_wallet_passes");

            migrationBuilder.AddColumn<string>(
                name: "push_token",
                table: "apple_devices",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "push_token",
                table: "apple_devices");

            migrationBuilder.AddColumn<string>(
                name: "push_token",
                table: "apple_wallet_passes",
                type: "text",
                nullable: true);
        }
    }
}
