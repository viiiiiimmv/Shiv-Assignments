using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApilnAsp.Migrations
{
    /// <inheritdoc />
    public partial class Adding_Identity_Tables_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "23c1c928-18d2-43ba-b817-1e626ab53015");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "a11b3721-c377-4188-9a2d-f57f04edea19");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "f9ae23c4-6acb-452c-9822-91f2866a5f4a");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "45280108-a98e-45f4-bf22-fc396850a44d", "2", "User", "User" },
                    { "6945afc5-7c80-4fe5-b5c7-964dce19ba99", "3", "HR", "HR" },
                    { "ef63dfb6-61c1-4a31-b135-a5a206ae83b5", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "45280108-a98e-45f4-bf22-fc396850a44d");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "6945afc5-7c80-4fe5-b5c7-964dce19ba99");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "ef63dfb6-61c1-4a31-b135-a5a206ae83b5");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "23c1c928-18d2-43ba-b817-1e626ab53015", "2", "User", "User" },
                    { "a11b3721-c377-4188-9a2d-f57f04edea19", "3", "HR", "HR" },
                    { "f9ae23c4-6acb-452c-9822-91f2866a5f4a", "1", "Admin", "Admin" }
                });
        }
    }
}
