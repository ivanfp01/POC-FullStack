using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Sql.Automoviles.Migrations
{
    /// <inheritdoc />
    public partial class Update_Automovil_With_VO_Fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Automovil_NumeroChasis",
                table: "Automovil");

            migrationBuilder.CreateIndex(
                name: "IX_Automovil_NumeroChasis",
                table: "Automovil",
                column: "NumeroChasis",
                unique: true,
                filter: "[NumeroChasis] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Automovil_NumeroChasis",
                table: "Automovil");

            migrationBuilder.CreateIndex(
                name: "IX_Automovil_NumeroChasis",
                table: "Automovil",
                column: "NumeroChasis",
                unique: true);
        }
    }
}
