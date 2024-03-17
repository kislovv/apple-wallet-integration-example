using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Rename_Db_Objects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "associated_store_apps");

            migrationBuilder.DropTable(
                name: "device_pass");

            migrationBuilder.DropTable(
                name: "partner_specifics");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropTable(
                name: "passes");

            migrationBuilder.CreateTable(
                name: "apple_devices",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apple_devices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "apple_wallet_partner_specifics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    background_color = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    icon_path = table.Column<string>(type: "text", nullable: false),
                    logo_path = table.Column<string>(type: "text", nullable: false),
                    strip_path = table.Column<string>(type: "text", nullable: false),
                    partner_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apple_wallet_partner_specifics", x => x.id);
                    table.ForeignKey(
                        name: "fk_apple_wallet_partner_specifics_partners_partner_id",
                        column: x => x.partner_id,
                        principalTable: "partners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "apple_wallet_passes",
                columns: table => new
                {
                    pass_id = table.Column<string>(type: "text", nullable: false),
                    last_updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    push_token = table.Column<string>(type: "text", nullable: false),
                    card_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apple_wallet_passes", x => x.pass_id);
                    table.ForeignKey(
                        name: "fk_apple_wallet_passes_cards_card_id",
                        column: x => x.card_id,
                        principalTable: "cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "apple_associated_store_apps",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    apple_wallet_partner_specific_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apple_associated_store_apps", x => x.id);
                    table.ForeignKey(
                        name: "fk_apple_associated_store_apps_apple_wallet_partner_specifics_",
                        column: x => x.apple_wallet_partner_specific_id,
                        principalTable: "apple_wallet_partner_specifics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "apple_device_apple_wallet_pass",
                columns: table => new
                {
                    apple_devices_id = table.Column<string>(type: "text", nullable: false),
                    apple_wallet_passes_pass_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apple_device_apple_wallet_pass", x => new { x.apple_devices_id, x.apple_wallet_passes_pass_id });
                    table.ForeignKey(
                        name: "fk_apple_device_apple_wallet_pass_apple_devices_apple_devices_",
                        column: x => x.apple_devices_id,
                        principalTable: "apple_devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_apple_device_apple_wallet_pass_apple_wallet_passes_apple_wa",
                        column: x => x.apple_wallet_passes_pass_id,
                        principalTable: "apple_wallet_passes",
                        principalColumn: "pass_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "apple_wallet_partner_specifics",
                columns: new[] { "id", "background_color", "description", "icon_path", "logo_path", "partner_id", "strip_path" },
                values: new object[] { 1L, "#5bd1e1", "Интенс APP", "Intens APP Icon 1x.png", "Intens APP Icon 1x.png", 1L, "Intens.png" });

            migrationBuilder.InsertData(
                table: "apple_associated_store_apps",
                columns: new[] { "id", "apple_wallet_partner_specific_id", "name" },
                values: new object[] { 1L, 1L, "Интенс APP" });

            migrationBuilder.CreateIndex(
                name: "ix_apple_associated_store_apps_apple_wallet_partner_specific_id",
                table: "apple_associated_store_apps",
                column: "apple_wallet_partner_specific_id");

            migrationBuilder.CreateIndex(
                name: "ix_apple_device_apple_wallet_pass_apple_wallet_passes_pass_id",
                table: "apple_device_apple_wallet_pass",
                column: "apple_wallet_passes_pass_id");

            migrationBuilder.CreateIndex(
                name: "ix_apple_wallet_partner_specifics_partner_id",
                table: "apple_wallet_partner_specifics",
                column: "partner_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_apple_wallet_passes_card_id",
                table: "apple_wallet_passes",
                column: "card_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "apple_associated_store_apps");

            migrationBuilder.DropTable(
                name: "apple_device_apple_wallet_pass");

            migrationBuilder.DropTable(
                name: "apple_wallet_partner_specifics");

            migrationBuilder.DropTable(
                name: "apple_devices");

            migrationBuilder.DropTable(
                name: "apple_wallet_passes");

            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_devices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "partner_specifics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    background_color = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    icon_path = table.Column<string>(type: "text", nullable: false),
                    logo_path = table.Column<string>(type: "text", nullable: false),
                    strip_path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_specifics", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_specifics_partners_partner_id",
                        column: x => x.partner_id,
                        principalTable: "partners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "passes",
                columns: table => new
                {
                    pass_id = table.Column<string>(type: "text", nullable: false),
                    card_id = table.Column<long>(type: "bigint", nullable: false),
                    last_updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    push_token = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_passes", x => x.pass_id);
                    table.ForeignKey(
                        name: "fk_passes_cards_card_id",
                        column: x => x.card_id,
                        principalTable: "cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "associated_store_apps",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partner_specific_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_associated_store_apps", x => x.id);
                    table.ForeignKey(
                        name: "fk_associated_store_apps_partner_specifics_partner_specific_id",
                        column: x => x.partner_specific_id,
                        principalTable: "partner_specifics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "device_pass",
                columns: table => new
                {
                    devices_id = table.Column<string>(type: "text", nullable: false),
                    passes_pass_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_device_pass", x => new { x.devices_id, x.passes_pass_id });
                    table.ForeignKey(
                        name: "fk_device_pass_devices_devices_id",
                        column: x => x.devices_id,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_device_pass_passes_passes_pass_id",
                        column: x => x.passes_pass_id,
                        principalTable: "passes",
                        principalColumn: "pass_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "partner_specifics",
                columns: new[] { "id", "background_color", "description", "icon_path", "logo_path", "partner_id", "strip_path" },
                values: new object[] { 1L, "#5bd1e1", "Интенс APP", "Intens APP Icon 1x.png", "Intens APP Icon 1x.png", 1L, "Intens.png" });

            migrationBuilder.InsertData(
                table: "associated_store_apps",
                columns: new[] { "id", "name", "partner_specific_id" },
                values: new object[] { 1L, "Интенс APP", 1L });

            migrationBuilder.CreateIndex(
                name: "ix_associated_store_apps_partner_specific_id",
                table: "associated_store_apps",
                column: "partner_specific_id");

            migrationBuilder.CreateIndex(
                name: "ix_device_pass_passes_pass_id",
                table: "device_pass",
                column: "passes_pass_id");

            migrationBuilder.CreateIndex(
                name: "ix_partner_specifics_partner_id",
                table: "partner_specifics",
                column: "partner_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_passes_card_id",
                table: "passes",
                column: "card_id",
                unique: true);
        }
    }
}
