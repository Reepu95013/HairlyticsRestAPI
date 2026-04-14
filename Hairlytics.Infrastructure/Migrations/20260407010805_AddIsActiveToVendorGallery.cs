using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hairlytics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToVendorGallery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "VendorGallery",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "VendorGallery");
        }
    }
}
