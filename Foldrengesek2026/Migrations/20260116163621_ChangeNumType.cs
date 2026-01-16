using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foldrengesek2026.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNumType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Magnitudo",
                table: "Naplok",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<decimal>(
                name: "Intenzitas",
                table: "Naplok",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Magnitudo",
                table: "Naplok",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");

            migrationBuilder.AlterColumn<double>(
                name: "Intenzitas",
                table: "Naplok",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");
        }
    }
}
