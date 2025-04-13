using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppO.Data.Migrations
{
    /// <inheritdoc />
    public partial class BanTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banimentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPermanente = table.Column<bool>(type: "bit", nullable: false),
                    DataDesbanimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminDesbanimentoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DataDesbanimentoDesbanimento = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banimentos_AspNetUsers_AdminDesbanimentoId",
                        column: x => x.AdminDesbanimentoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Banimentos_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Banimentos_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banimentos_AdminDesbanimentoId",
                table: "Banimentos",
                column: "AdminDesbanimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Banimentos_AdminId",
                table: "Banimentos",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Banimentos_UsuarioId",
                table: "Banimentos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banimentos");
        }
    }
}
