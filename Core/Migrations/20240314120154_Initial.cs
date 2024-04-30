using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company_Codes",
                columns: table => new
                {
                    Company_Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Codes", x => x.Company_Code);
                });

            migrationBuilder.CreateTable(
                name: "Company_Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Department_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Department_Id);
                });

            migrationBuilder.CreateTable(
                name: "Form_Statuses",
                columns: table => new
                {
                    Status_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Form_Statuses", x => x.Status_Id);
                });

            migrationBuilder.CreateTable(
                name: "Organization_Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type_of_Organization = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Purchase_Organizations",
                columns: table => new
                {
                    PO_Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase_Organizations", x => x.PO_Code);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Role_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Role_Id);
                });

            migrationBuilder.CreateTable(
                name: "Type_of_Addresses",
                columns: table => new
                {
                    Address_Type_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_of_Addresses", x => x.Address_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Type_of_Contacts",
                columns: table => new
                {
                    Contact_Type_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contact_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_of_Contacts", x => x.Contact_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Type_of_Tankers",
                columns: table => new
                {
                    Tanker_Type_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tanker_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_of_Tankers", x => x.Tanker_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Type_of_Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vendor_Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_of_Vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Role_Mappings",
                columns: table => new
                {
                    Employee_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Role_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Role_Mappings", x => new { x.Employee_Id, x.Role_Id });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Employee_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    First_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Middle_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Last_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile_No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Reporting_Manager_EmpId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Employee_Id);
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Form_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vendor_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Status_Id = table.Column<int>(type: "int", nullable: false),
                    Vendor_Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vendor_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vendor_Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vendor_Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company_Code = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Department_Id = table.Column<int>(type: "int", nullable: false),
                    Purchase_Organization = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Created_On = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created_By = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Form_Id);
                    table.ForeignKey(
                        name: "FK_Forms_Company_Codes_Company_Code",
                        column: x => x.Company_Code,
                        principalTable: "Company_Codes",
                        principalColumn: "Company_Code");
                    table.ForeignKey(
                        name: "FK_Forms_Departments_Department_Id",
                        column: x => x.Department_Id,
                        principalTable: "Departments",
                        principalColumn: "Department_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Forms_Form_Statuses_Status_Id",
                        column: x => x.Status_Id,
                        principalTable: "Form_Statuses",
                        principalColumn: "Status_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Forms_Purchase_Organizations_Purchase_Organization",
                        column: x => x.Purchase_Organization,
                        principalTable: "Purchase_Organizations",
                        principalColumn: "PO_Code");
                    table.ForeignKey(
                        name: "FK_Forms_Type_of_Vendors_Vendor_Type_Id",
                        column: x => x.Vendor_Type_Id,
                        principalTable: "Type_of_Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlows",
                columns: table => new
                {
                    WorkFlow_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role_Id = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Vendor_Type_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlows", x => x.WorkFlow_Id);
                    table.ForeignKey(
                        name: "FK_WorkFlows_Roles_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Roles",
                        principalColumn: "Role_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkFlows_Type_of_Vendors_Vendor_Type_Id",
                        column: x => x.Vendor_Type_Id,
                        principalTable: "Type_of_Vendors",
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
                    AddressData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Contact_Type_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Address_Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_Type_of_Addresses_Contact_Type_Id",
                        column: x => x.Contact_Type_Id,
                        principalTable: "Type_of_Addresses",
                        principalColumn: "Address_Type_Id");
                });

            migrationBuilder.CreateTable(
                name: "Annual_TurnOver",
                columns: table => new
                {
                    TurnOver_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    SalesTurnOver = table.Column<long>(type: "bigint", nullable: false),
                    OperatingProfit = table.Column<long>(type: "bigint", nullable: false),
                    NetProfit = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annual_TurnOver", x => x.TurnOver_Id);
                    table.ForeignKey(
                        name: "FK_Annual_TurnOver_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attchments",
                columns: table => new
                {
                    Attachment_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    File_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File_Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File_Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Is_Expiry_Available = table.Column<bool>(type: "bit", nullable: false),
                    Expiry_Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attchments", x => x.Attachment_Id);
                    table.ForeignKey(
                        name: "FK_Attchments_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bank_Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    AccountHolder = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Bank = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Account_Number = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IFSC = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    SWIFT = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    IBAN = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bank_Details_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commercial_Profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Financial_Credit_Rating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Agency_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GSTIN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    MSME_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MSME_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceCategory = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commercial_Profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commercial_Profile_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Contact_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Contact_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone_Number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Mobile_Number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Contact_Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contacts_Type_of_Contacts_Contact_Type_Id",
                        column: x => x.Contact_Type_Id,
                        principalTable: "Type_of_Contacts",
                        principalColumn: "Contact_Type_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Domestic_Organization_Profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Type_of_Org_Id = table.Column<int>(type: "int", nullable: false),
                    Status_of_Company_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domestic_Organization_Profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Domestic_Organization_Profile_Company_Statuses_Status_of_Company_Id",
                        column: x => x.Status_of_Company_Id,
                        principalTable: "Company_Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Domestic_Organization_Profile_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Domestic_Organization_Profile_Organization_Types_Type_of_Org_Id",
                        column: x => x.Type_of_Org_Id,
                        principalTable: "Organization_Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Process_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Created_By = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created_On = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Completed_On = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Process_Id);
                    table.ForeignKey(
                        name: "FK_Processes_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_Users_Created_By",
                        column: x => x.Created_By,
                        principalTable: "Users",
                        principalColumn: "Employee_Id");
                });

            migrationBuilder.CreateTable(
                name: "Proprietor_or_Partners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PercentageShare = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proprietor_or_Partners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proprietor_or_Partners_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tanker_Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Tanker_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Capacity_of_Tanker = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tanker_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tanker_Details_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tanker_Details_Type_of_Tankers_Tanker_Type_Id",
                        column: x => x.Tanker_Type_Id,
                        principalTable: "Type_of_Tankers",
                        principalColumn: "Tanker_Type_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Task_Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Owner_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Task_Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Technical_Profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Is_ISO_Cerified = table.Column<bool>(type: "bit", nullable: false),
                    Other_Qms_Certified = table.Column<bool>(type: "bit", nullable: false),
                    Planning_for_Qms = table.Column<bool>(type: "bit", nullable: false),
                    Is_Statutory_Provisions_Adheard = table.Column<bool>(type: "bit", nullable: false),
                    Initiatives_for_Development = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technical_Profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Technical_Profile_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionHistories",
                columns: table => new
                {
                    Log_Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logged_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionHistories", x => x.Log_Id);
                    table.ForeignKey(
                        name: "FK_TransactionHistories_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transport_Vendor_Personal_Data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Name_of_Transporter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_of_Establishment = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "VendorBranches",
                columns: table => new
                {
                    Branch_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form_Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile_No = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorBranches", x => x.Branch_Id);
                    table.ForeignKey(
                        name: "FK_VendorBranches_Forms_Form_Id",
                        column: x => x.Form_Id,
                        principalTable: "Forms",
                        principalColumn: "Form_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Contact_Type_Id",
                table: "Addresses",
                column: "Contact_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Form_Id",
                table: "Addresses",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Annual_TurnOver_Form_Id",
                table: "Annual_TurnOver",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Attchments_Form_Id",
                table: "Attchments",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_Details_Form_Id",
                table: "Bank_Details",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Commercial_Profile_Form_Id",
                table: "Commercial_Profile",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Contact_Type_Id",
                table: "Contacts",
                column: "Contact_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Form_Id",
                table: "Contacts",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Domestic_Organization_Profile_Form_Id",
                table: "Domestic_Organization_Profile",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Domestic_Organization_Profile_Status_of_Company_Id",
                table: "Domestic_Organization_Profile",
                column: "Status_of_Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Domestic_Organization_Profile_Type_of_Org_Id",
                table: "Domestic_Organization_Profile",
                column: "Type_of_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Domestic_Vendor_Personal_Data_Form_Id",
                table: "Domestic_Vendor_Personal_Data",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_Company_Code",
                table: "Forms",
                column: "Company_Code");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_Department_Id",
                table: "Forms",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_Purchase_Organization",
                table: "Forms",
                column: "Purchase_Organization");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_Status_Id",
                table: "Forms",
                column: "Status_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_Vendor_Type_Id",
                table: "Forms",
                column: "Vendor_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_Created_By",
                table: "Processes",
                column: "Created_By");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_Form_Id",
                table: "Processes",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Proprietor_or_Partners_Form_Id",
                table: "Proprietor_or_Partners",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tanker_Details_Form_Id",
                table: "Tanker_Details",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tanker_Details_Tanker_Type_Id",
                table: "Tanker_Details",
                column: "Tanker_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Form_Id",
                table: "Tasks",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Technical_Profile_Form_Id",
                table: "Technical_Profile",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistories_Form_Id",
                table: "TransactionHistories",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transport_Vendor_Personal_Data_Form_Id",
                table: "Transport_Vendor_Personal_Data",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_VendorBranches_Form_Id",
                table: "VendorBranches",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlows_Role_Id",
                table: "WorkFlows",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlows_Vendor_Type_Id",
                table: "WorkFlows",
                column: "Vendor_Type_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Annual_TurnOver");

            migrationBuilder.DropTable(
                name: "Attchments");

            migrationBuilder.DropTable(
                name: "Bank_Details");

            migrationBuilder.DropTable(
                name: "Commercial_Profile");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Domestic_Organization_Profile");

            migrationBuilder.DropTable(
                name: "Domestic_Vendor_Personal_Data");

            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropTable(
                name: "Proprietor_or_Partners");

            migrationBuilder.DropTable(
                name: "Tanker_Details");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Technical_Profile");

            migrationBuilder.DropTable(
                name: "TransactionHistories");

            migrationBuilder.DropTable(
                name: "Transport_Vendor_Personal_Data");

            migrationBuilder.DropTable(
                name: "User_Role_Mappings");

            migrationBuilder.DropTable(
                name: "VendorBranches");

            migrationBuilder.DropTable(
                name: "WorkFlows");

            migrationBuilder.DropTable(
                name: "Type_of_Addresses");

            migrationBuilder.DropTable(
                name: "Type_of_Contacts");

            migrationBuilder.DropTable(
                name: "Company_Statuses");

            migrationBuilder.DropTable(
                name: "Organization_Types");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Type_of_Tankers");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Company_Codes");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Form_Statuses");

            migrationBuilder.DropTable(
                name: "Purchase_Organizations");

            migrationBuilder.DropTable(
                name: "Type_of_Vendors");
        }
    }
}
