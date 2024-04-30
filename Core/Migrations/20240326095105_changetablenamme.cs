using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class changetablenamme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Domestic_Organization_Profile_Company_Statuses_Status_of_Company_Id",
                table: "Domestic_Organization_Profile");

            migrationBuilder.DropForeignKey(
                name: "FK_Domestic_Organization_Profile_Forms_Form_Id",
                table: "Domestic_Organization_Profile");

            migrationBuilder.DropForeignKey(
                name: "FK_Domestic_Organization_Profile_Organization_Types_Type_of_Org_Id",
                table: "Domestic_Organization_Profile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Domestic_Organization_Profile",
                table: "Domestic_Organization_Profile");

            migrationBuilder.RenameTable(
                name: "Domestic_Organization_Profile",
                newName: "Vendor_Organization_Profile");

            migrationBuilder.RenameIndex(
                name: "IX_Domestic_Organization_Profile_Type_of_Org_Id",
                table: "Vendor_Organization_Profile",
                newName: "IX_Vendor_Organization_Profile_Type_of_Org_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Domestic_Organization_Profile_Status_of_Company_Id",
                table: "Vendor_Organization_Profile",
                newName: "IX_Vendor_Organization_Profile_Status_of_Company_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Domestic_Organization_Profile_Form_Id",
                table: "Vendor_Organization_Profile",
                newName: "IX_Vendor_Organization_Profile_Form_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendor_Organization_Profile",
                table: "Vendor_Organization_Profile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_Organization_Profile_Company_Statuses_Status_of_Company_Id",
                table: "Vendor_Organization_Profile",
                column: "Status_of_Company_Id",
                principalTable: "Company_Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_Organization_Profile_Forms_Form_Id",
                table: "Vendor_Organization_Profile",
                column: "Form_Id",
                principalTable: "Forms",
                principalColumn: "Form_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_Organization_Profile_Organization_Types_Type_of_Org_Id",
                table: "Vendor_Organization_Profile",
                column: "Type_of_Org_Id",
                principalTable: "Organization_Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_Organization_Profile_Company_Statuses_Status_of_Company_Id",
                table: "Vendor_Organization_Profile");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_Organization_Profile_Forms_Form_Id",
                table: "Vendor_Organization_Profile");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_Organization_Profile_Organization_Types_Type_of_Org_Id",
                table: "Vendor_Organization_Profile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendor_Organization_Profile",
                table: "Vendor_Organization_Profile");

            migrationBuilder.RenameTable(
                name: "Vendor_Organization_Profile",
                newName: "Domestic_Organization_Profile");

            migrationBuilder.RenameIndex(
                name: "IX_Vendor_Organization_Profile_Type_of_Org_Id",
                table: "Domestic_Organization_Profile",
                newName: "IX_Domestic_Organization_Profile_Type_of_Org_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Vendor_Organization_Profile_Status_of_Company_Id",
                table: "Domestic_Organization_Profile",
                newName: "IX_Domestic_Organization_Profile_Status_of_Company_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Vendor_Organization_Profile_Form_Id",
                table: "Domestic_Organization_Profile",
                newName: "IX_Domestic_Organization_Profile_Form_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Domestic_Organization_Profile",
                table: "Domestic_Organization_Profile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Domestic_Organization_Profile_Company_Statuses_Status_of_Company_Id",
                table: "Domestic_Organization_Profile",
                column: "Status_of_Company_Id",
                principalTable: "Company_Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Domestic_Organization_Profile_Forms_Form_Id",
                table: "Domestic_Organization_Profile",
                column: "Form_Id",
                principalTable: "Forms",
                principalColumn: "Form_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Domestic_Organization_Profile_Organization_Types_Type_of_Org_Id",
                table: "Domestic_Organization_Profile",
                column: "Type_of_Org_Id",
                principalTable: "Organization_Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
