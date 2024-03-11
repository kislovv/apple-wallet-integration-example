using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Seed_AssociatedApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "associated_store_apps",
                columns: new[] { "id", "name", "partner_specific_id" },
                values: new object[] { 1L, "Интенс APP", 1L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "associated_store_apps",
                keyColumn: "id",
                keyValue: 1L);
        }
    }
}
