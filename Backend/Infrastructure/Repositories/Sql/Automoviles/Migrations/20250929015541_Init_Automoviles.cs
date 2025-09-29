using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Sql.Automoviles.Migrations
{
    /// <inheritdoc />
    public partial class Init_Automoviles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Automovil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marca = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    NumeroChasis = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    NumeroMotor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Automovil", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Automovil_NumeroChasis",
                table: "Automovil",
                column: "NumeroChasis",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Automovil_NumeroMotor",
                table: "Automovil",
                column: "NumeroMotor",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Automovil");
        }
    }
}
