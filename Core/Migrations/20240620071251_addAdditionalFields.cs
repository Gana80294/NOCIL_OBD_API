using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class addAdditionalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Industry_Id = table.Column<int>(type: "int", nullable: false),
                    Incoterms_Id = table.Column<int>(type: "int", nullable: false),
                    Reconciliation_Id = table.Column<int>(type: "int", nullable: false),
                    Schema_Id = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order_Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrBased = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SrvBased = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Search_Term = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalFields_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdditionalFields_Incoterms_Incoterms_Id",
                        column: x => x.Incoterms_Id,
                        principalTable: "Incoterms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdditionalFields_Industry_Industry_Id",
                        column: x => x.Industry_Id,
                        principalTable: "Industry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdditionalFields_ReconciliationAccounts_Reconciliation_Id",
                        column: x => x.Reconciliation_Id,
                        principalTable: "ReconciliationAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdditionalFields_SchemaGroups_Schema_Id",
                        column: x => x.Schema_Id,
                        principalTable: "SchemaGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Address_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Address_Type_Id = table.Column<int>(type: "int", nullable: false),
                    House_No = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Street_2 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Street_3 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Street_4 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    District = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Postal_Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    City = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Country_Code = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Region_Id = table.Column<int>(type: "int", nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Address_Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Country_Country_Code",
                        column: x => x.Country_Code,
                        principalTable: "Country",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Addresses_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_Region_Region_Id",
                        column: x => x.Region_Id,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_Type_of_Addresses_Address_Type_Id",
                        column: x => x.Address_Type_Id,
                        principalTable: "Type_of_Addresses",
                        principalColumn: "Address_Type_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transport_Vendor_Personal_Data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Title_Id = table.Column<int>(type: "int", nullable: false),
                    Name_of_Transporter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_of_Establishment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GSTVenClass_Id = table.Column<int>(type: "int", nullable: false),
                    No_of_Own_Vehicles = table.Column<int>(type: "int", nullable: false),
                    No_of_Drivers = table.Column<int>(type: "int", nullable: false),
                    Nicerglobe_Registration_Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transport_Vendor_Personal_Data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transport_Vendor_Personal_Data_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transport_Vendor_Personal_Data_GSTVenClass_GSTVenClass_Id",
                        column: x => x.GSTVenClass_Id,
                        principalTable: "GSTVenClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transport_Vendor_Personal_Data_Titles_Title_Id",
                        column: x => x.Title_Id,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vendor_Personal_Data",
                columns: table => new
                {
                    Personal_Info_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Organization_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plant_Installation_Year = table.Column<int>(type: "int", nullable: false),
                    Title_Id = table.Column<int>(type: "int", nullable: false),
                    GSTVenClass_Id = table.Column<int>(type: "int", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Vendor_Personal_Data_GSTVenClass_GSTVenClass_Id",
                        column: x => x.GSTVenClass_Id,
                        principalTable: "GSTVenClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vendor_Personal_Data_Titles_Title_Id",
                        column: x => x.Title_Id,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFields_Form_Id",
                table: "AdditionalFields",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFields_Incoterms_Id",
                table: "AdditionalFields",
                column: "Incoterms_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFields_Industry_Id",
                table: "AdditionalFields",
                column: "Industry_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFields_Reconciliation_Id",
                table: "AdditionalFields",
                column: "Reconciliation_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFields_Schema_Id",
                table: "AdditionalFields",
                column: "Schema_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Address_Type_Id",
                table: "Addresses",
                column: "Address_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Country_Code",
                table: "Addresses",
                column: "Country_Code");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Form_Id",
                table: "Addresses",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Region_Id",
                table: "Addresses",
                column: "Region_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transport_Vendor_Personal_Data_Form_Id",
                table: "Transport_Vendor_Personal_Data",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transport_Vendor_Personal_Data_GSTVenClass_Id",
                table: "Transport_Vendor_Personal_Data",
                column: "GSTVenClass_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transport_Vendor_Personal_Data_Title_Id",
                table: "Transport_Vendor_Personal_Data",
                column: "Title_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_Personal_Data_Form_Id",
                table: "Vendor_Personal_Data",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_Personal_Data_GSTVenClass_Id",
                table: "Vendor_Personal_Data",
                column: "GSTVenClass_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_Personal_Data_Title_Id",
                table: "Vendor_Personal_Data",
                column: "Title_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalFields");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Transport_Vendor_Personal_Data");

            migrationBuilder.DropTable(
                name: "Vendor_Personal_Data");
        }
    }
}
