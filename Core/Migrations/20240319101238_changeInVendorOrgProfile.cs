using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class changeInVendorOrgProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Annual_Prod_Capacity",
                table: "Domestic_Organization_Profile",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "RelationToNocil",
                table: "Domestic_Organization_Profile",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Annual_Prod_Capacity",
                table: "Domestic_Organization_Profile");

            migrationBuilder.DropColumn(
                name: "RelationToNocil",
                table: "Domestic_Organization_Profile");
        }
    }
}
