using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddInterestToBillingCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Interest",
                table: "BillingCycle",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interest",
                table: "BillingCycle");
        }
    }
}
