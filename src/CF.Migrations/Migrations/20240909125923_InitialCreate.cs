using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VirtualCard",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirtualCard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BillingCycle",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VirtualCardId = table.Column<long>(type: "bigint", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WithdrawalsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingCycle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingCycle_VirtualCard_VirtualCardId",
                        column: x => x.VirtualCardId,
                        principalTable: "VirtualCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillingCycle_VirtualCardId",
                table: "BillingCycle",
                column: "VirtualCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VirtualCard_CardNumber",
                table: "VirtualCard",
                column: "CardNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillingCycle");

            migrationBuilder.DropTable(
                name: "VirtualCard");
        }
    }
}
