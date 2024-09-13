using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddVirtualCardIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "VirtualCard",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "VirtualCard");
        }
    }
}
