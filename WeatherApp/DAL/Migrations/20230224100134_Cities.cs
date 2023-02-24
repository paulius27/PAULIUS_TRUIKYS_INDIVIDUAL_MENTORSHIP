using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Cities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityName",
                table: "WeatherHistory");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "WeatherHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherHistory_CityId",
                table: "WeatherHistory",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherHistory_Cities_CityId",
                table: "WeatherHistory",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherHistory_Cities_CityId",
                table: "WeatherHistory");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_WeatherHistory_CityId",
                table: "WeatherHistory");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "WeatherHistory");

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "WeatherHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
