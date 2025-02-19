using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthPassport.DAL.Migrations
{
    /// <inheritdoc />
    public partial class createAntropologicalResearchesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AntropologicalResearches",
                columns: table => new
                {
                    AntropologicalResearchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntropologicalResearches", x => x.AntropologicalResearchId);
                    table.ForeignKey(
                        name: "FK_AntropologicalResearches_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AntropologicalResearches_EmployeeId",
                table: "AntropologicalResearches",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AntropologicalResearches");
        }
    }
}
