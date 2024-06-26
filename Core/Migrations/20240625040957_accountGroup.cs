using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class accountGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountGroup_Id",
                table: "AdditionalFields",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VendorAccountGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Is_Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorAccountGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFields_AccountGroup_Id",
                table: "AdditionalFields",
                column: "AccountGroup_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalFields_VendorAccountGroups_AccountGroup_Id",
                table: "AdditionalFields",
                column: "AccountGroup_Id",
                principalTable: "VendorAccountGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalFields_VendorAccountGroups_AccountGroup_Id",
                table: "AdditionalFields");

            migrationBuilder.DropTable(
                name: "VendorAccountGroups");

            migrationBuilder.DropIndex(
                name: "IX_AdditionalFields_AccountGroup_Id",
                table: "AdditionalFields");

            migrationBuilder.DropColumn(
                name: "AccountGroup_Id",
                table: "AdditionalFields");
        }
    }
}
