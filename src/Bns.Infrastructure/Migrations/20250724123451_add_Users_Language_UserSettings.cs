using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bns.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_Users_Language_UserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_settings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_id = table.Column<int>(type: "integer", nullable: true),
                    minimalize_menu = table.Column<bool>(type: "boolean", nullable: false),
                    undo_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    load_notes = table.Column<bool>(type: "boolean", nullable: false),
                    display_zeros = table.Column<bool>(type: "boolean", nullable: false),
                    supress_null_rows = table.Column<bool>(type: "boolean", nullable: false),
                    highlight_active_row = table.Column<bool>(type: "boolean", nullable: false),
                    remove_null_rows_in_shanpshot = table.Column<bool>(type: "boolean", nullable: false),
                    prefer_user_panels = table.Column<bool>(type: "boolean", nullable: false),
                    automatic_calculation = table.Column<bool>(type: "boolean", nullable: false),
                    startup_dashboard = table.Column<string>(type: "text", nullable: false),
                    display_panel_code = table.Column<bool>(type: "boolean", nullable: false),
                    modify_version_facts = table.Column<bool>(type: "boolean", nullable: false),
                    dimension_view_display_type = table.Column<string>(type: "text", nullable: false),
                    skin = table.Column<string>(type: "text", nullable: false),
                    use_current_date = table.Column<string>(type: "text", nullable: false),
                    dimension_presets = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_settings_Languages_language_id",
                        column: x => x.language_id,
                        principalTable: "Languages",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    locked = table.Column<bool>(type: "boolean", nullable: false),
                    delete_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    create_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    settings_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_user_settings_settings_id",
                        column: x => x.settings_id,
                        principalTable: "user_settings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_settings_language_id",
                table: "user_settings",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_settings_id",
                table: "users",
                column: "settings_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "user_settings");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
