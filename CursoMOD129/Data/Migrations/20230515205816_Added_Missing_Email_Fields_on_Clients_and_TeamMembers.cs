using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CursoMOD129.Data.Migrations
{
    /// <inheritdoc />
    public partial class Added_Missing_Email_Fields_on_Clients_and_TeamMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "TeamMembers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "diogothesilva@gmail.com");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clients",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "diogothesilva@gmail.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clients");
        }
    }
}
