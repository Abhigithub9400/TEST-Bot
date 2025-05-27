using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediAssist.DbContext.Migrations
{
    /// <inheritdoc />
    public partial class FhirMappingTableEntityIdTypeChangedToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EntityId",
                table: "FHIRStoreMapping",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EntityId",
                table: "FHIRStoreMapping",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
