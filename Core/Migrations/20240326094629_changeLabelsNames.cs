using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class changeLabelsNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Domestic_Vendor_Personal_Data");

            migrationBuilder.CreateTable(
                name: "Vendor_Personal_Data",
                columns: table => new
                {
                    Personal_Info_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Organization_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plant_Installation_Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor_Personal_Data", x => x.Personal_Info_Id);
                    table.ForeignKey(
                        name: "FK_Vendor_Personal_Data_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_Personal_Data_Form_Id",
                table: "Vendor_Personal_Data",
                column: "Form_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vendor_Personal_Data");

            migrationBuilder.CreateTable(
                name: "Domestic_Vendor_Personal_Data",
                columns: table => new
                {
                    Domestic_Personal_Info_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Organization_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plant_Installation_Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domestic_Vendor_Personal_Data", x => x.Domestic_Personal_Info_Id);
                    table.ForeignKey(
                        name: "FK_Domestic_Vendor_Personal_Data_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Domestic_Vendor_Personal_Data_Form_Id",
                table: "Domestic_Vendor_Personal_Data",
                column: "Form_Id");
        }
    }
}
