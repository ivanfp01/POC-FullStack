using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Sql.Automoviles.Migrations
{
    /// <inheritdoc />
    public partial class HardConstraints_Automovil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Reparar datos existentes (placeholders / nulls / año 0)
            migrationBuilder.Sql(@"
                UPDATE [Automovil] SET [Marca]  = 'DESCONOCIDA' WHERE [Marca]  IS NULL OR LTRIM(RTRIM([Marca]))  IN ('', 'string', 'STRING', 'String');
                UPDATE [Automovil] SET [Modelo] = 'DESCONOCIDO' WHERE [Modelo] IS NULL OR LTRIM(RTRIM([Modelo])) IN ('', 'string', 'STRING', 'String');
                UPDATE [Automovil] SET [Color]  = 'SIN COLOR'   WHERE [Color]  IS NULL OR LTRIM(RTRIM([Color]))  IN ('', 'string', 'STRING', 'String');
                UPDATE [Automovil] SET [Año]    = YEAR(GETDATE()) WHERE [Año] IS NULL OR [Año] < 1900 OR [Año] > YEAR(GETDATE()) + 1 OR [Año] = 0;
            ");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Automovil_Año_Rango",
                table: "Automovil",
                sql: "[Año] >= 1900 AND [Año] <= (YEAR(GETDATE()) + 1)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Automovil_Color_NotBlank",
                table: "Automovil",
                sql: "LEN(LTRIM(RTRIM([Color]))) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Automovil_Marca_NotBlank",
                table: "Automovil",
                sql: "LEN(LTRIM(RTRIM([Marca]))) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Automovil_Modelo_NotBlank",
                table: "Automovil",
                sql: "LEN(LTRIM(RTRIM([Modelo]))) > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Automovil_Año_Rango",
                table: "Automovil");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Automovil_Color_NotBlank",
                table: "Automovil");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Automovil_Marca_NotBlank",
                table: "Automovil");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Automovil_Modelo_NotBlank",
                table: "Automovil");

            // EF revertirá los check constraints, dejamos sin limpieza reversa de datos.
        }
    }
}
