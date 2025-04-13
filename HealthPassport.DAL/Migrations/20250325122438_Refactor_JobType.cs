using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthPassport.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_JobType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isCanEditItems",
                table: "JobTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCanEditItems",
                table: "JobTypes");
        }
    }
}
