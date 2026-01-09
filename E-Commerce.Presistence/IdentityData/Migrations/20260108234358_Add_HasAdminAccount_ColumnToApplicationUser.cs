using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Presistence.IdentityData.Migrations
{
    /// <inheritdoc />
    public partial class Add_HasAdminAccount_ColumnToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAdminAccount",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAdminAccount",
                table: "Users");
        }
    }
}
