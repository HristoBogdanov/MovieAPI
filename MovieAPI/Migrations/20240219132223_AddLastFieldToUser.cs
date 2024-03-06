using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLastFieldToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "a51e87c5-8f9f-4a2b-9269-a49141ef383e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1be6dd61-9dc2-4bb9-9190-5afdff6fbcd3", "AQAAAAIAAYagAAAAEDfbgxMgtmw2lNpKuoe9VBaPRVB6eoCTkE5Szr3kRWKcB2u4yEM04/RStYhMeiiM3A==", "372aea5f-93ca-4df3-920e-fce1e29be7da" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a7871881-00e6-4ee2-996c-42b8426ba556", "AQAAAAIAAYagAAAAEAtTegp0kIH6pHWwocDTDNS+gOHsYNCe8XwO2deSgc1flJiGVZYaJAb6mAfwAw+1gQ==", "" });
        }
    }
}
