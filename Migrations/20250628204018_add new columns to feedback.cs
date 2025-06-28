using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alwalid.Cms.Api.Migrations
{
    public partial class addnewcolumnstofeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnglishFullName",
                table: "Feedbacks",
                newName: "EnglishName");

            migrationBuilder.RenameColumn(
                name: "ArabicFullName",
                table: "Feedbacks",
                newName: "ArabicName");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Feedbacks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Feedbacks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Feedbacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Feedbacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Feedbacks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "EnglishName",
                table: "Feedbacks",
                newName: "EnglishFullName");

            migrationBuilder.RenameColumn(
                name: "ArabicName",
                table: "Feedbacks",
                newName: "ArabicFullName");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
