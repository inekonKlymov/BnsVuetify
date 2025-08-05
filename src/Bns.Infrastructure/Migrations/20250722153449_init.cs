using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bns.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "data_source_olap_config",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    impersonate = table.Column<bool>(type: "boolean", nullable: false),
                    load_members_count = table.Column<int>(type: "integer", nullable: false),
                    fiscal_offset = table.Column<int>(type: "integer", nullable: false),
                    use_fiscal_offset = table.Column<bool>(type: "boolean", nullable: false),
                    pool_size = table.Column<int>(type: "integer", nullable: false),
                    load_member_code_properties = table.Column<bool>(type: "boolean", nullable: false),
                    use_olap_notes = table.Column<bool>(type: "boolean", nullable: false),
                    allow_writeback_on_aggregation_level = table.Column<bool>(type: "boolean", nullable: false),
                    writeback_leaf_limit = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_source_olap_config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "data_source",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    connection_string = table.Column<string>(type: "text", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConfigurationId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_source", x => x.id);
                    table.ForeignKey(
                        name: "FK_data_source_data_source_olap_config_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "data_source_olap_config",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_data_source_ConfigurationId",
                table: "data_source",
                column: "ConfigurationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_source");

            migrationBuilder.DropTable(
                name: "data_source_olap_config");
        }
    }
}
