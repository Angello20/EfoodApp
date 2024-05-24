using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfoodApp.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTarjetaProcesadorPagoMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TarjetasProcesadorPago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TarjetaId = table.Column<int>(type: "int", nullable: false),
                    ProcesadorPagoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarjetasProcesadorPago", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TarjetasProcesadorPago_ProcesadoresPago_ProcesadorPagoId",
                        column: x => x.ProcesadorPagoId,
                        principalTable: "ProcesadoresPago",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TarjetasProcesadorPago_Tarjetas_TarjetaId",
                        column: x => x.TarjetaId,
                        principalTable: "Tarjetas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TarjetasProcesadorPago_ProcesadorPagoId",
                table: "TarjetasProcesadorPago",
                column: "ProcesadorPagoId");

            migrationBuilder.CreateIndex(
                name: "IX_TarjetasProcesadorPago_TarjetaId",
                table: "TarjetasProcesadorPago",
                column: "TarjetaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TarjetasProcesadorPago");
        }
    }
}
