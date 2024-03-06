using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLastFieldToRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "59aa6d82-7139-44bd-b4a4-b64c09113698");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a7871881-00e6-4ee2-996c-42b8426ba556", "AQAAAAIAAYagAAAAEAtTegp0kIH6pHWwocDTDNS+gOHsYNCe8XwO2deSgc1flJiGVZYaJAb6mAfwAw+1gQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a158dff4-8a75-408b-8e73-48682e29f084", "AQAAAAIAAYagAAAAENp6ufUH3p8OY60jgUWKkX6kHI8sB8PvvFPKH0zquFG/VnaJeqRXDwpwvUCs2d58gA==" });
        }
    }
}
