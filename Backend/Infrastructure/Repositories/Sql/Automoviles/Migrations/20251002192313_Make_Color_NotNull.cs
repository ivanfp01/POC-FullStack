using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Sql.Automoviles.Migrations
{
    /// <inheritdoc />
    public partial class Make_Color_NotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Primero actualizar registros NULL a 'SIN COLOR'
            migrationBuilder.Sql("UPDATE [Automovil] SET [Color] = 'SIN COLOR' WHERE [Color] IS NULL;");

            // Luego alterar columna a NOT NULL
            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Automovil",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Automovil",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
