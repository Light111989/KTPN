using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddBienCheHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BienCheHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BienCheId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenDonVi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SLVienChuc = table.Column<byte>(type: "tinyint", nullable: false),
                    SLHopDong = table.Column<byte>(type: "tinyint", nullable: false),
                    SLHopDongND = table.Column<byte>(type: "tinyint", nullable: false),
                    SLBoTri = table.Column<byte>(type: "tinyint", nullable: false),
                    SoQuyetDinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SLGiaoVien = table.Column<byte>(type: "tinyint", nullable: false),
                    SLQuanLy = table.Column<byte>(type: "tinyint", nullable: false),
                    SLNhanVien = table.Column<byte>(type: "tinyint", nullable: false),
                    SLHD111 = table.Column<byte>(type: "tinyint", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienCheHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BienCheHistories_BienChes_BienCheId",
                        column: x => x.BienCheId,
                        principalTable: "BienChes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BienCheHistories_BienCheId",
                table: "BienCheHistories",
                column: "BienCheId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BienCheHistories");
        }
    }
}
