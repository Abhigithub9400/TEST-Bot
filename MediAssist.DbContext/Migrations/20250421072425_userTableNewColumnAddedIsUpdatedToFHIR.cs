using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediAssist.DbContext.Migrations
{
    /// <inheritdoc />
    public partial class userTableNewColumnAddedIsUpdatedToFHIR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUpdatedtoFHIR",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUpdatedtoFHIR",
                table: "Users");
        }
    }
}
