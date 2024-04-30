using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class changeForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Type_of_Addresses_Contact_Type_Id",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_Contact_Type_Id",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Contact_Type_Id",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Address_Type_Id",
                table: "Addresses",
                column: "Address_Type_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Type_of_Addresses_Address_Type_Id",
                table: "Addresses",
                column: "Address_Type_Id",
                principalTable: "Type_of_Addresses",
                principalColumn: "Address_Type_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Type_of_Addresses_Address_Type_Id",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_Address_Type_Id",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "Contact_Type_Id",
                table: "Addresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Contact_Type_Id",
                table: "Addresses",
                column: "Contact_Type_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Type_of_Addresses_Contact_Type_Id",
                table: "Addresses",
                column: "Contact_Type_Id",
                principalTable: "Type_of_Addresses",
                principalColumn: "Address_Type_Id");
        }
    }
}
