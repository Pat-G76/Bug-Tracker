using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bug_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class DeletedCommentCreatedDataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeDetails",
                table: "Comment");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreated",
                table: "Comment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeCreated",
                table: "Comment");

            migrationBuilder.AddColumn<string>(
                name: "TimeDetails",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
