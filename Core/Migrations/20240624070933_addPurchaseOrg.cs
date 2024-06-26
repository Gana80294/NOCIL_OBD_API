using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class addPurchaseOrg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PO_Code",
                table: "AdditionalFields",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFields_PO_Code",
                table: "AdditionalFields",
                column: "PO_Code");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalFields_Purchase_Organizations_PO_Code",
                table: "AdditionalFields",
                column: "PO_Code",
                principalTable: "Purchase_Organizations",
                principalColumn: "PO_Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalFields_Purchase_Organizations_PO_Code",
                table: "AdditionalFields");

            migrationBuilder.DropIndex(
                name: "IX_AdditionalFields_PO_Code",
                table: "AdditionalFields");

            migrationBuilder.DropColumn(
                name: "PO_Code",
                table: "AdditionalFields");
        }
    }
}
