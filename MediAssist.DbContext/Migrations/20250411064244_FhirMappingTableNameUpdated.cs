using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediAssist.DbContext.Migrations
{
    /// <inheritdoc />
    public partial class FhirMappingTableNameUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FhirStoreMapping",
                table: "FhirStoreMapping");

            migrationBuilder.RenameTable(
                name: "FhirStoreMapping",
                newName: "FHIRStoreMapping");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FHIRStoreMapping",
                table: "FHIRStoreMapping",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FHIRStoreMapping",
                table: "FHIRStoreMapping");

            migrationBuilder.RenameTable(
                name: "FHIRStoreMapping",
                newName: "FhirStoreMapping");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FhirStoreMapping",
                table: "FhirStoreMapping",
                column: "Id");
        }
    }
}
