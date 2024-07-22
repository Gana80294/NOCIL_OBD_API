using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class addNullableProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalFields_Incoterms_Incoterms_Id",
                table: "AdditionalFields");

            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalFields_SchemaGroups_Schema_Id",
                table: "AdditionalFields");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_Personal_Data_GSTVenClass_GSTVenClass_Id",
                table: "Vendor_Personal_Data");

            migrationBuilder.AlterColumn<int>(
                name: "GSTVenClass_Id",
                table: "Vendor_Personal_Data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Schema_Id",
                table: "AdditionalFields",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Incoterms_Id",
                table: "AdditionalFields",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalFields_Incoterms_Incoterms_Id",
                table: "AdditionalFields",
                column: "Incoterms_Id",
                principalTable: "Incoterms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalFields_SchemaGroups_Schema_Id",
                table: "AdditionalFields",
                column: "Schema_Id",
                principalTable: "SchemaGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_Personal_Data_GSTVenClass_GSTVenClass_Id",
                table: "Vendor_Personal_Data",
                column: "GSTVenClass_Id",
                principalTable: "GSTVenClass",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalFields_Incoterms_Incoterms_Id",
                table: "AdditionalFields");

            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalFields_SchemaGroups_Schema_Id",
                table: "AdditionalFields");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_Personal_Data_GSTVenClass_GSTVenClass_Id",
                table: "Vendor_Personal_Data");

            migrationBuilder.AlterColumn<int>(
                name: "GSTVenClass_Id",
                table: "Vendor_Personal_Data",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Schema_Id",
                table: "AdditionalFields",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Incoterms_Id",
                table: "AdditionalFields",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalFields_Incoterms_Incoterms_Id",
                table: "AdditionalFields",
                column: "Incoterms_Id",
                principalTable: "Incoterms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalFields_SchemaGroups_Schema_Id",
                table: "AdditionalFields",
                column: "Schema_Id",
                principalTable: "SchemaGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_Personal_Data_GSTVenClass_GSTVenClass_Id",
                table: "Vendor_Personal_Data",
                column: "GSTVenClass_Id",
                principalTable: "GSTVenClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
