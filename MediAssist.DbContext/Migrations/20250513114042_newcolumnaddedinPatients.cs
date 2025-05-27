using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediAssist.DbContext.Migrations
{
    /// <inheritdoc />
    public partial class newcolumnaddedinPatients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DOB",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DOB",
                table: "Patients");
        }
    }
}
