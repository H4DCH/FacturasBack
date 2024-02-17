using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facturas2.Migrations
{
    /// <inheritdoc />
    public partial class FacturaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Facturas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_UsuarioId",
                table: "Facturas",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_AspNetUsers_UsuarioId",
                table: "Facturas",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_AspNetUsers_UsuarioId",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_UsuarioId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Facturas");
        }
    }
}
