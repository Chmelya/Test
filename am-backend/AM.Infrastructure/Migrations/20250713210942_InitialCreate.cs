using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recclass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recclass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meteorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecclassId = table.Column<int>(type: "int", nullable: false),
                    Mass = table.Column<double>(type: "float", nullable: false),
                    Fall = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Reclat = table.Column<double>(type: "float", nullable: false),
                    Reclong = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meteorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meteorites_Recclass_RecclassId",
                        column: x => x.RecclassId,
                        principalTable: "Recclass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Geolocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeteoriteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geolocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Geolocation_Meteorites_MeteoriteId",
                        column: x => x.MeteoriteId,
                        principalTable: "Meteorites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Geolocation_MeteoriteId",
                table: "Geolocation",
                column: "MeteoriteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_Name",
                table: "Meteorites",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_RecclassId",
                table: "Meteorites",
                column: "RecclassId");

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_Year",
                table: "Meteorites",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_Recclass_Name",
                table: "Recclass",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Geolocation");

            migrationBuilder.DropTable(
                name: "Meteorites");

            migrationBuilder.DropTable(
                name: "Recclass");
        }
    }
}
