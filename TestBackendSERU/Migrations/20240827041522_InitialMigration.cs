using Microsoft.EntityFrameworkCore.Migrations;

namespace TestBackendSERU.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_At = table.Column<string>(nullable: true),
                    Updated_At = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Is_Admin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle_Brand",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_At = table.Column<string>(nullable: true),
                    Updated_At = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle_Brand", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle_Year",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_At = table.Column<string>(nullable: true),
                    Updated_At = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle_Year", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle_Type",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_At = table.Column<string>(nullable: true),
                    Updated_At = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Brand_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle_Type", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Vehicle_Type_Vehicle_Brand_Brand_ID",
                        column: x => x.Brand_ID,
                        principalTable: "Vehicle_Brand",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle_Model",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_At = table.Column<string>(nullable: true),
                    Updated_At = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle_Model", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Vehicle_Model_Vehicle_Type_Type_ID",
                        column: x => x.Type_ID,
                        principalTable: "Vehicle_Type",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Pricelist",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_At = table.Column<string>(nullable: true),
                    Updated_At = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    Year_ID = table.Column<int>(nullable: true),
                    Model_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pricelist", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pricelist_Vehicle_Model_Model_ID",
                        column: x => x.Model_ID,
                        principalTable: "Vehicle_Model",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Pricelist_Vehicle_Year_Year_ID",
                        column: x => x.Year_ID,
                        principalTable: "Vehicle_Year",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pricelist_Model_ID",
                table: "Pricelist",
                column: "Model_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Pricelist_Year_ID",
                table: "Pricelist",
                column: "Year_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Model_Type_ID",
                table: "Vehicle_Model",
                column: "Type_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Type_Brand_ID",
                table: "Vehicle_Type",
                column: "Brand_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pricelist");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Vehicle_Model");

            migrationBuilder.DropTable(
                name: "Vehicle_Year");

            migrationBuilder.DropTable(
                name: "Vehicle_Type");

            migrationBuilder.DropTable(
                name: "Vehicle_Brand");
        }
    }
}
