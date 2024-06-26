using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class updateAccountGroupId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalFields_VendorAccountGroups_AccountGroup_Code",
                table: "AdditionalFields");

            migrationBuilder.RenameColumn(
                name: "AccountGroup_Code",
                table: "AdditionalFields",
                newName: "AccountGroup_Id");

            migrationBuilder.RenameIndex(
                name: "IX_AdditionalFields_AccountGroup_Code",
                table: "AdditionalFields",
                newName: "IX_AdditionalFields_AccountGroup_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalFields_VendorAccountGroups_AccountGroup_Id",
                table: "AdditionalFields",
                column: "AccountGroup_Id",
                principalTable: "VendorAccountGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalFields_VendorAccountGroups_AccountGroup_Id",
                table: "AdditionalFields");

            migrationBuilder.RenameColumn(
                name: "AccountGroup_Id",
                table: "AdditionalFields",
                newName: "AccountGroup_Code");

            migrationBuilder.RenameIndex(
                name: "IX_AdditionalFields_AccountGroup_Id",
                table: "AdditionalFields",
                newName: "IX_AdditionalFields_AccountGroup_Code");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalFields_VendorAccountGroups_AccountGroup_Code",
                table: "AdditionalFields",
                column: "AccountGroup_Code",
                principalTable: "VendorAccountGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
