using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class updateNullableForTransport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transport_Vendor_Personal_Data_GSTVenClass_GSTVenClass_Id",
                table: "Transport_Vendor_Personal_Data");

            migrationBuilder.AlterColumn<int>(
                name: "GSTVenClass_Id",
                table: "Transport_Vendor_Personal_Data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_of_Establishment",
                table: "Transport_Vendor_Personal_Data",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Transport_Vendor_Personal_Data_GSTVenClass_GSTVenClass_Id",
                table: "Transport_Vendor_Personal_Data",
                column: "GSTVenClass_Id",
                principalTable: "GSTVenClass",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transport_Vendor_Personal_Data_GSTVenClass_GSTVenClass_Id",
                table: "Transport_Vendor_Personal_Data");

            migrationBuilder.AlterColumn<int>(
                name: "GSTVenClass_Id",
                table: "Transport_Vendor_Personal_Data",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_of_Establishment",
                table: "Transport_Vendor_Personal_Data",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transport_Vendor_Personal_Data_GSTVenClass_GSTVenClass_Id",
                table: "Transport_Vendor_Personal_Data",
                column: "GSTVenClass_Id",
                principalTable: "GSTVenClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
