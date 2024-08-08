using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class addVehicleDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicle_Details",
                columns: table => new
                {
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Vehicle_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capacity = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle_Details", x => x.VehicleTypeId);
                    table.ForeignKey(
                        name: "FK_Vehicle_Details_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Details_Form_Id",
                table: "Vehicle_Details",
                column: "Form_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicle_Details");
        }
    }
}
