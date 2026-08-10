using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7d760404-f999-492c-a970-cc88bf73d150", "e8fb0f17-bf71-4613-99dc-8c9d82b97a49", "USER", "USER" },
                    { "f04a5784-5d5a-484a-8335-281d56a0da48", "cb162ed7-f666-4d0d-b76f-7386a147a883", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d760404-f999-492c-a970-cc88bf73d150");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f04a5784-5d5a-484a-8335-281d56a0da48");
        }
    }
}
