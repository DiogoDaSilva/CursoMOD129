using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CursoMOD129.Data.Migrations
{
    /// <inheritdoc />
    public partial class Added_Missing_ZipCode_to_Clients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Clients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Clients");
        }
    }
}
