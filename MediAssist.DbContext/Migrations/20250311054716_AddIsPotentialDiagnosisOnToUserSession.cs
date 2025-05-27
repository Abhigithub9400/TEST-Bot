using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediAssist.DbContext.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPotentialDiagnosisOnToUserSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPotentialDiagnosisOn",
                table: "UserSession",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPotentialDiagnosisOn",
                table: "UserSession");
        }
    }
}
