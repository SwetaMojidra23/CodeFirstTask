using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeFirstTask.Migrations
{
    /// <inheritdoc />
    public partial class CodeTaskMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Hobbies",
                table: "Students",
                newName: "hobbies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "hobbies",
                table: "Students",
                newName: "Hobbies");
        }
    }
}
