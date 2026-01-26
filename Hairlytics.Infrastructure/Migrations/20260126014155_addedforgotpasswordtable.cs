using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hairlytics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedforgotpasswordtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForgotPassword",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OTP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Revoke = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForgotPassword", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForgotPassword");
        }
    }
}
