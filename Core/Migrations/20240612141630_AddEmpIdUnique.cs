using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddEmpIdUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employee_Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    First_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_Employee_Id", x => x.Employee_Id);
                });

            migrationBuilder.CreateTable(
                name: "ForgotPasswordOtpTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employee_Id = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    TxId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Requested_On = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Validated_On = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForgotPasswordOtpTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForgotPasswordOtpTransactions_Users_Employee_Id",
                        column: x => x.Employee_Id,
                        principalTable: "Users",
                        principalColumn: "Employee_Id", onDelete: ReferentialAction.NoAction);
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
                    Owner_Id = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Role_Id = table.Column<int>(type: "int", nullable: true),
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
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tasks_Roles_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Roles",
                        principalColumn: "Role_Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_Owner_Id",
                        column: x => x.Owner_Id,
                        principalTable: "Users",
                        principalColumn: "Employee_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForgotPasswordOtpTransactions_Employee_Id",
                table: "ForgotPasswordOtpTransactions",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Form_Id",
                table: "Tasks",
                column: "Form_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Owner_Id",
                table: "Tasks",
                column: "Owner_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Role_Id",
                table: "Tasks",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Employee_Id",
                table: "Users",
                column: "Employee_Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForgotPasswordOtpTransactions");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
