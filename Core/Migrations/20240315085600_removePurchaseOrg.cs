using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class removePurchaseOrg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Purchase_Organizations_Purchase_Organization",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_Purchase_Organization",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "Purchase_Organization",
                table: "Forms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Purchase_Organization",
                table: "Forms",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_Purchase_Organization",
                table: "Forms",
                column: "Purchase_Organization");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Purchase_Organizations_Purchase_Organization",
                table: "Forms",
                column: "Purchase_Organization",
                principalTable: "Purchase_Organizations",
                principalColumn: "PO_Code");
        }
    }
}
