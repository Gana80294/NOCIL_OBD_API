using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOCIL_VP.Domain.Core.Migrations
{
    /// <inheritdoc />
    public partial class addQueryFilters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Attchments_Forms_Form_Id",
            //    table: "Attchments");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Attchments",
            //    table: "Attchments");

            //migrationBuilder.RenameTable(
            //    name: "Attchments",
            //    newName: "Attachments");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Attchments_Form_Id",
            //    table: "Attachments",
            //    newName: "IX_Attachments_Form_Id");

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Type_of_Vendors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Type_of_Tankers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Type_of_Contacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Type_of_Addresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Purchase_Organizations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Organization_Types",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Form_Statuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Company_Statuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Company_Codes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Attachments",
            //    table: "Attachments",
            //    column: "Attachment_Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Attachments_Forms_Form_Id",
            //    table: "Attachments",
            //    column: "Form_Id",
            //    principalTable: "Forms",
            //    principalColumn: "Form_Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Forms_Form_Id",
                table: "Attachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Type_of_Vendors");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Type_of_Tankers");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Type_of_Contacts");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Type_of_Addresses");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Purchase_Organizations");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Organization_Types");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Form_Statuses");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Company_Statuses");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Company_Codes");

            migrationBuilder.RenameTable(
                name: "Attachments",
                newName: "Attchments");

            migrationBuilder.RenameIndex(
                name: "IX_Attachments_Form_Id",
                table: "Attchments",
                newName: "IX_Attchments_Form_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attchments",
                table: "Attchments",
                column: "Attachment_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attchments_Forms_Form_Id",
                table: "Attchments",
                column: "Form_Id",
                principalTable: "Forms",
                principalColumn: "Form_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
