using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediAssist.DbContext.Migrations
{
    /// <inheritdoc />
    public partial class removed_unwanted_code : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MedicalCredentials",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MedicalCredentials",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MedicalCredentials",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MedicalCredentials",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MedicalCredentials",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MedicalCredentials",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MedicalCredentials",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MedicalCredentials",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MedicalCredentials",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MedicalCredentials",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: 7);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "CreatedDate" },
                values: new object[,]
                {
                    { 1, "Usability", new DateTime(2024, 12, 27, 18, 28, 58, 393, DateTimeKind.Local).AddTicks(286) },
                    { 2, "Performance", new DateTime(2024, 12, 27, 18, 28, 58, 393, DateTimeKind.Local).AddTicks(302) },
                    { 3, "Features", new DateTime(2024, 12, 27, 18, 28, 58, 393, DateTimeKind.Local).AddTicks(303) },
                    { 4, "Bugs", new DateTime(2024, 12, 27, 18, 28, 58, 393, DateTimeKind.Local).AddTicks(304) },
                    { 5, "Other", new DateTime(2024, 12, 27, 18, 28, 58, 393, DateTimeKind.Local).AddTicks(305) }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Gender" },
                values: new object[,]
                {
                    { 1, "Male" },
                    { 2, "Female" },
                    { 3, "Transgender" },
                    { 4, "Non-binary" },
                    { 5, "Prefer Not to Say" }
                });

            migrationBuilder.InsertData(
                table: "MedicalCredentials",
                columns: new[] { "Id", "MedicalCredentials" },
                values: new object[,]
                {
                    { 1, "MD (Doctor of Medicine)" },
                    { 2, "MBBS (Bachelor of Medicine, Bachelor of Surgery)" },
                    { 3, "DO (Doctor of Osteopathic Medicine)" },
                    { 4, "BDS (Bachelor of Dental Surgery)" },
                    { 5, "MCh (Master of Surgery)" },
                    { 6, "DM (Doctorate of Medicine)" },
                    { 7, "FRCS (Fellowship of the Royal College of Surgeons)" },
                    { 8, "FACP (Fellow of the American College of Physicians)" },
                    { 9, "MS (Master of Surgery)" },
                    { 10, "DNB (Diplomate of National Board)" }
                });

            migrationBuilder.InsertData(
                table: "UserTitles",
                columns: new[] { "Id", "Abbreviations", "Title" },
                values: new object[,]
                {
                    { 1, "Dr.", "Dr. (Doctor)" },
                    { 2, "Cons.", "Consultant" },
                    { 3, "Res.", "Resident" },
                    { 4, "Att.", "Attending Physician" },
                    { 5, "Sr. Cons.", "Senior Consultant" },
                    { 6, "Ch. Surg.", "Chief Surgeon" },
                    { 7, "Clin. Lead", "Clinical Lead" }
                });
        }
    }
}
