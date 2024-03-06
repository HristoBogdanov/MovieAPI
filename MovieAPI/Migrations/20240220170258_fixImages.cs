using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAPI.Migrations
{
    /// <inheritdoc />
    public partial class fixImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                value: "1225faae-3519-44fb-9fe6-a5bb76dafb05");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f7f84cfa-a40f-4df9-ae3b-9461d514baea", "AQAAAAIAAYagAAAAEF4e+E3j+i+zaLf9LdP86aPaa+CTwGfugUt52/HjOmqGgaq3kI0+uETNXrFw3spaXA==", "df9fca8f-d605-4d30-be10-d550af67a857" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
