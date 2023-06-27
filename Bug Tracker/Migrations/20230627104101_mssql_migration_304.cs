using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bug_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class mssql_migration_304 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_EmployeeId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEmployee_AspNetUsers_EmployeeId",
                table: "ProjectEmployee");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "ProjectEmployee",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Comment",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_EmployeeId",
                table: "Comment",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEmployee_AspNetUsers_EmployeeId",
                table: "ProjectEmployee",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_EmployeeId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEmployee_AspNetUsers_EmployeeId",
                table: "ProjectEmployee");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "ProjectEmployee",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Comment",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_EmployeeId",
                table: "Comment",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEmployee_AspNetUsers_EmployeeId",
                table: "ProjectEmployee",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
