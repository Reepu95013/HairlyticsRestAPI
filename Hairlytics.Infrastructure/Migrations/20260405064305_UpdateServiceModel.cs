using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hairlytics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_VendorProfiles_VendorProfileId",
                table: "Service");

            migrationBuilder.DropTable(
                name: "ServiceImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Service",
                table: "Service");

            migrationBuilder.RenameTable(
                name: "Service",
                newName: "Services");

            migrationBuilder.RenameColumn(
                name: "MainImage",
                table: "Services",
                newName: "Image");

            migrationBuilder.RenameIndex(
                name: "IX_Service_VendorProfileId",
                table: "Services",
                newName: "IX_Services_VendorProfileId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Services",
                table: "Services",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "VendorGallery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorProfileId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsThumbnail = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorGallery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorGallery_VendorProfiles_VendorProfileId",
                        column: x => x.VendorProfileId,
                        principalTable: "VendorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_CategoryId",
                table: "Services",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorGallery_VendorProfileId",
                table: "VendorGallery",
                column: "VendorProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Category_CategoryId",
                table: "Services",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_VendorProfiles_VendorProfileId",
                table: "Services",
                column: "VendorProfileId",
                principalTable: "VendorProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Category_CategoryId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_VendorProfiles_VendorProfileId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "VendorGallery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_CategoryId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Services");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "Service");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Service",
                newName: "MainImage");

            migrationBuilder.RenameIndex(
                name: "IX_Services_VendorProfileId",
                table: "Service",
                newName: "IX_Service_VendorProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Service",
                table: "Service",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ServiceImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceImage_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceImage_ServiceId",
                table: "ServiceImage",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_VendorProfiles_VendorProfileId",
                table: "Service",
                column: "VendorProfileId",
                principalTable: "VendorProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
