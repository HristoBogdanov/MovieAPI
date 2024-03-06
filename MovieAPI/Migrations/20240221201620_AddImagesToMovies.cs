using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddImagesToMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoviesImages",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesImages", x => new { x.MovieId, x.ImageId });
                    table.ForeignKey(
                        name: "FK_MoviesImages_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesImages_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "0a708ab5-bf47-4304-86c6-d2d380a6627d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d93a48b7-32e2-4502-a28b-66d09a5a6e8c", "AQAAAAIAAYagAAAAENj6xQg0/kooi+t/brqYQoQI9aX4LKpeypHLLmeem9g/wi3Aqd/oBOaL6SvAchMF1Q==", "a1ac2ddd-54e4-4718-acb8-6e1a9d446574" });

            migrationBuilder.CreateIndex(
                name: "IX_MoviesImages_ImageId",
                table: "MoviesImages",
                column: "ImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviesImages");

            migrationBuilder.DropTable(
                name: "Images");

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
    }
}
