using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class yearOfEstablishment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date_of_Establishment",
                table: "Transport_Vendor_Personal_Data");

            migrationBuilder.AddColumn<int>(
                name: "Year_of_Establishment",
                table: "Transport_Vendor_Personal_Data",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year_of_Establishment",
                table: "Transport_Vendor_Personal_Data");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date_of_Establishment",
                table: "Transport_Vendor_Personal_Data",
                type: "datetime2",
                nullable: true);
        }
    }
}
