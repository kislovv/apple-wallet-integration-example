using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Seed_PartnerSpecific : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "partner_specifics",
                columns: new[] { "id", "background_color", "description", "icon_path", "logo_path", "partner_id", "strip_path" },
                values: new object[] { 1L, "#5bd1e1", "Интенс APP", "Intens APP Icon 1x.png", "Intens APP Icon 1x.png", 1L, "Intens.png" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "partner_specifics",
                keyColumn: "id",
                keyValue: 1L);
        }
    }
}
