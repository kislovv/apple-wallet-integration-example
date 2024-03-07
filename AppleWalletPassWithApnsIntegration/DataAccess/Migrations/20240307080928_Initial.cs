using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "partners",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    partner_specific_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.UniqueConstraint("ak_users_login", x => x.login);
                });

            migrationBuilder.CreateTable(
                name: "partner_specifics",
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
                    table.PrimaryKey("pk_partner_specifics", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_specifics_partners_partner_id",
                        column: x => x.partner_id,
                        principalTable: "partners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "participants",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    balance = table.Column<decimal>(type: "numeric(2)", nullable: false, defaultValue: 0.00m),
                    card_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_participants", x => x.id);
                    table.ForeignKey(
                        name: "fk_participants_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "associated_store_apps",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    partner_specific_id = table.Column<long>(type: "bigint", nullable: false)
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
                name: "cards",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_hash_id = table.Column<string>(type: "text", nullable: false),
                    participant_id = table.Column<long>(type: "bigint", nullable: false),
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    pass_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cards", x => x.id);
                    table.ForeignKey(
                        name: "fk_cards_participants_participant_id",
                        column: x => x.participant_id,
                        principalTable: "participants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cards_partners_partner_id",
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
                    card_id = table.Column<long>(type: "bigint", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "ix_associated_store_apps_partner_specific_id",
                table: "associated_store_apps",
                column: "partner_specific_id");

            migrationBuilder.CreateIndex(
                name: "ix_cards_participant_id",
                table: "cards",
                column: "participant_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_cards_partner_id",
                table: "cards",
                column: "partner_id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "cards");

            migrationBuilder.DropTable(
                name: "participants");

            migrationBuilder.DropTable(
                name: "partners");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
