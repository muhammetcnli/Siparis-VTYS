using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace alisveris.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kullanici",
                columns: table => new
                {
                    KullaniciID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EPosta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sifre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanici_KullaniciID", x => x.KullaniciID);
                    table.CheckConstraint("CK_Kullanici_Rol", "[Rol] IN ('Admin', 'Kullanıcı')");
                });

            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    UrunID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StokAdedi = table.Column<int>(type: "int", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urun_UrunID", x => x.UrunID);
                    table.CheckConstraint("CK_Urun_Fiyat_Positive", "[Fiyat] >= 0");
                    table.CheckConstraint("CK_Urun_StokAdedi_Positive", "[StokAdedi] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Odeme",
                columns: table => new
                {
                    OdemeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciID = table.Column<int>(type: "int", nullable: false),
                    OdemeTuru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OdemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OdemeDurumu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odeme_OdemeID", x => x.OdemeID);
                    table.CheckConstraint("CK_Odeme_OdemeDurumu", "[OdemeDurumu] IN (0, 1)");
                    table.CheckConstraint("CK_Odeme_OdemeTarihi", "[OdemeTarihi] <= GETDATE()");
                    table.ForeignKey(
                        name: "FK_Odeme_Kullanici_KullaniciID",
                        column: x => x.KullaniciID,
                        principalTable: "Kullanici",
                        principalColumn: "KullaniciID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Siparis",
                columns: table => new
                {
                    SiparisID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciID = table.Column<int>(type: "int", nullable: false),
                    ToplamFiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SiparisTarihi = table.Column<DateTime>(type: "datetime", nullable: false),
                    TeslimTarihi = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siparis_SiparisID", x => x.SiparisID);
                    table.ForeignKey(
                        name: "FK_Siparis_Kullanici_KullaniciID",
                        column: x => x.KullaniciID,
                        principalTable: "Kullanici",
                        principalColumn: "KullaniciID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kargo",
                columns: table => new
                {
                    KargoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiparisID = table.Column<int>(type: "int", nullable: false),
                    TakipNo = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Durum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KargoSirketi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kargo_KargoID", x => x.KargoID);
                    table.CheckConstraint("CK_Kargo_TakipNo_Length", "LEN(TakipNo) = 6");
                    table.ForeignKey(
                        name: "FK_Kargo_Siparis_SiparisID",
                        column: x => x.SiparisID,
                        principalTable: "Siparis",
                        principalColumn: "SiparisID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiparisDetaylari",
                columns: table => new
                {
                    DetayID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiparisID = table.Column<int>(type: "int", nullable: false),
                    Miktar = table.Column<int>(type: "int", nullable: false),
                    AraToplam = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisDetayi_DetayID", x => x.DetayID);
                    table.CheckConstraint("CK_SiparisDetayi_AraToplam", "[AraToplam] >= 0");
                    table.CheckConstraint("CK_SiparisDetayi_Miktar", "[Miktar] > 0");
                    table.ForeignKey(
                        name: "FK_SiparisDetayi_Siparis_SiparisID",
                        column: x => x.SiparisID,
                        principalTable: "Siparis",
                        principalColumn: "SiparisID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Icerir",
                columns: table => new
                {
                    IcerirID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunID = table.Column<int>(type: "int", nullable: false),
                    DetayID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icerir_IcerirID", x => x.IcerirID);
                    table.ForeignKey(
                        name: "FK_Icerir_SiparisDetayi_DetayID",
                        column: x => x.DetayID,
                        principalTable: "SiparisDetaylari",
                        principalColumn: "DetayID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Icerir_Urun_UrunID",
                        column: x => x.UrunID,
                        principalTable: "Urunler",
                        principalColumn: "UrunID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Icerir_DetayID",
                table: "Icerir",
                column: "DetayID");

            migrationBuilder.CreateIndex(
                name: "IX_Icerir_UrunID",
                table: "Icerir",
                column: "UrunID");

            migrationBuilder.CreateIndex(
                name: "IX_Kargo_SiparisID",
                table: "Kargo",
                column: "SiparisID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kargo_TakipNo",
                table: "Kargo",
                column: "TakipNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_EPosta",
                table: "Kullanici",
                column: "EPosta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Odeme_KullaniciID",
                table: "Odeme",
                column: "KullaniciID");

            migrationBuilder.CreateIndex(
                name: "IX_Siparis_KullaniciID",
                table: "Siparis",
                column: "KullaniciID");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisDetaylari_SiparisID",
                table: "SiparisDetaylari",
                column: "SiparisID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Icerir");

            migrationBuilder.DropTable(
                name: "Kargo");

            migrationBuilder.DropTable(
                name: "Odeme");

            migrationBuilder.DropTable(
                name: "SiparisDetaylari");

            migrationBuilder.DropTable(
                name: "Urunler");

            migrationBuilder.DropTable(
                name: "Siparis");

            migrationBuilder.DropTable(
                name: "Kullanici");
        }
    }
}
