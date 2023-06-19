using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bug_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class IssueTypeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IssueTypeID",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TypeID",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "IssueType",
                columns: table => new
                {
                    IssueTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueType", x => x.IssueTypeID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TypeID",
                table: "Ticket",
                column: "TypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_IssueType_TypeID",
                table: "Ticket",
                column: "TypeID",
                principalTable: "IssueType",
                principalColumn: "IssueTypeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_IssueType_TypeID",
                table: "Ticket");

            migrationBuilder.DropTable(
                name: "IssueType");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TypeID",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "IssueTypeID",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TypeID",
                table: "Ticket");
        }
    }
}
