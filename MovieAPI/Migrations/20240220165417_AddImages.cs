using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFolder",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrls",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "6de0379d-56a3-4eef-be12-b09faa5f2a1e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ddbcafcc-3a62-47cc-bd37-777ae7533231", "AQAAAAIAAYagAAAAENcpn44unnSpE29YhMGKekXpTU9b/LPSLvtT6auysuE/nqjr8r0OxKWz4uh/On6TIQ==", "6a1d09c8-5ec0-4a36-a777-745c54532ffd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFolder",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ImageUrls",
                table: "Movies");

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
    }
}
