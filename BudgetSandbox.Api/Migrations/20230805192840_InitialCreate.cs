using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BudgetSandbox.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sandboxs",
                columns: table => new
                {
                    sandbox_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sandboxs", x => x.sandbox_id);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    account_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    balance = table.Column<decimal>(type: "numeric", nullable: false),
                    positive = table.Column<bool>(type: "boolean", nullable: false),
                    sandbox_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounts", x => x.account_id);
                    table.ForeignKey(
                        name: "fk_accounts_sandboxs_sandbox_id",
                        column: x => x.sandbox_id,
                        principalTable: "sandboxs",
                        principalColumn: "sandbox_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    asset_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    amount_value = table.Column<decimal>(type: "numeric", nullable: false),
                    sandbox_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assets", x => x.asset_id);
                    table.ForeignKey(
                        name: "fk_assets_sandboxs_sandbox_id",
                        column: x => x.sandbox_id,
                        principalTable: "sandboxs",
                        principalColumn: "sandbox_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "buckets",
                columns: table => new
                {
                    bucket_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    balance = table.Column<decimal>(type: "numeric", nullable: false),
                    goal_balance = table.Column<decimal>(type: "numeric", nullable: true),
                    goal_achieved = table.Column<bool>(type: "boolean", nullable: false),
                    positive = table.Column<bool>(type: "boolean", nullable: false),
                    archived = table.Column<bool>(type: "boolean", nullable: false),
                    sandbox_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_buckets", x => x.bucket_id);
                    table.ForeignKey(
                        name: "fk_buckets_sandboxs_sandbox_id",
                        column: x => x.sandbox_id,
                        principalTable: "sandboxs",
                        principalColumn: "sandbox_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_sandboxs",
                columns: table => new
                {
                    user_sandbox_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    sandbox_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_sandboxs", x => x.user_sandbox_id);
                    table.ForeignKey(
                        name: "fk_user_sandboxs_sandboxs_sandbox_id",
                        column: x => x.sandbox_id,
                        principalTable: "sandboxs",
                        principalColumn: "sandbox_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cash_flow_items",
                columns: table => new
                {
                    cash_flow_item_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    frequency = table.Column<string>(type: "text", nullable: false),
                    positive = table.Column<bool>(type: "boolean", nullable: false),
                    sandbox_id = table.Column<int>(type: "integer", nullable: false),
                    asset_id = table.Column<int>(type: "integer", nullable: true),
                    account_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cash_flow_items", x => x.cash_flow_item_id);
                    table.ForeignKey(
                        name: "fk_cash_flow_items_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id");
                    table.ForeignKey(
                        name: "fk_cash_flow_items_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "asset_id");
                    table.ForeignKey(
                        name: "fk_cash_flow_items_sandboxs_sandbox_id",
                        column: x => x.sandbox_id,
                        principalTable: "sandboxs",
                        principalColumn: "sandbox_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_buckets",
                columns: table => new
                {
                    account_bucket_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    account_id = table.Column<int>(type: "integer", nullable: false),
                    bucket_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: true),
                    percent = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_buckets", x => x.account_bucket_id);
                    table.ForeignKey(
                        name: "fk_account_buckets_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_account_buckets_buckets_bucket_id",
                        column: x => x.bucket_id,
                        principalTable: "buckets",
                        principalColumn: "bucket_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cash_flow_item_accounts",
                columns: table => new
                {
                    cash_flow_item_account_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cash_flow_item_id = table.Column<int>(type: "integer", nullable: false),
                    account_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: true),
                    percent = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cash_flow_item_accounts", x => x.cash_flow_item_account_id);
                    table.ForeignKey(
                        name: "fk_cash_flow_item_accounts_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cash_flow_item_accounts_cash_flow_items_cash_flow_item_id",
                        column: x => x.cash_flow_item_id,
                        principalTable: "cash_flow_items",
                        principalColumn: "cash_flow_item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cash_flow_item_buckets",
                columns: table => new
                {
                    cash_flow_item_bucket_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cash_flow_item_id = table.Column<int>(type: "integer", nullable: false),
                    bucket_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: true),
                    percent = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cash_flow_item_buckets", x => x.cash_flow_item_bucket_id);
                    table.ForeignKey(
                        name: "fk_cash_flow_item_buckets_buckets_bucket_id",
                        column: x => x.bucket_id,
                        principalTable: "buckets",
                        principalColumn: "bucket_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cash_flow_item_buckets_cash_flow_items_cash_flow_item_id",
                        column: x => x.cash_flow_item_id,
                        principalTable: "cash_flow_items",
                        principalColumn: "cash_flow_item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_account_buckets_account_id",
                table: "account_buckets",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_buckets_bucket_id",
                table: "account_buckets",
                column: "bucket_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_sandbox_id",
                table: "accounts",
                column: "sandbox_id");

            migrationBuilder.CreateIndex(
                name: "ix_assets_sandbox_id",
                table: "assets",
                column: "sandbox_id");

            migrationBuilder.CreateIndex(
                name: "ix_buckets_sandbox_id",
                table: "buckets",
                column: "sandbox_id");

            migrationBuilder.CreateIndex(
                name: "ix_cash_flow_item_accounts_account_id",
                table: "cash_flow_item_accounts",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_cash_flow_item_accounts_cash_flow_item_id",
                table: "cash_flow_item_accounts",
                column: "cash_flow_item_id");

            migrationBuilder.CreateIndex(
                name: "ix_cash_flow_item_buckets_bucket_id",
                table: "cash_flow_item_buckets",
                column: "bucket_id");

            migrationBuilder.CreateIndex(
                name: "ix_cash_flow_item_buckets_cash_flow_item_id",
                table: "cash_flow_item_buckets",
                column: "cash_flow_item_id");

            migrationBuilder.CreateIndex(
                name: "ix_cash_flow_items_account_id",
                table: "cash_flow_items",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_cash_flow_items_asset_id",
                table: "cash_flow_items",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_cash_flow_items_sandbox_id",
                table: "cash_flow_items",
                column: "sandbox_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_sandboxs_sandbox_id",
                table: "user_sandboxs",
                column: "sandbox_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_buckets");

            migrationBuilder.DropTable(
                name: "cash_flow_item_accounts");

            migrationBuilder.DropTable(
                name: "cash_flow_item_buckets");

            migrationBuilder.DropTable(
                name: "user_sandboxs");

            migrationBuilder.DropTable(
                name: "buckets");

            migrationBuilder.DropTable(
                name: "cash_flow_items");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "assets");

            migrationBuilder.DropTable(
                name: "sandboxs");
        }
    }
}
