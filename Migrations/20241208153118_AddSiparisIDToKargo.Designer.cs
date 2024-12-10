﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace alisveris.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241208153118_AddSiparisIDToKargo")]
    partial class AddSiparisIDToKargo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Odeme", b =>
                {
                    b.Property<int>("OdemeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OdemeID"));

                    b.Property<int>("KullaniciID")
                        .HasColumnType("int")
                        .HasColumnName("KullaniciID");

                    b.Property<bool>("OdemeDurumu")
                        .HasColumnType("bit")
                        .HasColumnName("OdemeDurumu");

                    b.Property<DateTime>("OdemeTarihi")
                        .HasColumnType("datetime2")
                        .HasColumnName("OdemeTarihi");

                    b.Property<string>("OdemeTuru")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("OdemeTuru");

                    b.HasKey("OdemeID")
                        .HasName("PK_Odeme_OdemeID");

                    b.HasIndex("KullaniciID");

                    b.ToTable("Odeme", t =>
                        {
                            t.HasCheckConstraint("CK_Odeme_OdemeDurumu", "[OdemeDurumu] IN (0, 1)");

                            t.HasCheckConstraint("CK_Odeme_OdemeTarihi", "[OdemeTarihi] <= GETDATE()");
                        });
                });

            modelBuilder.Entity("alisveris.Models.Icerir", b =>
                {
                    b.Property<int>("IcerirID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IcerirID"));

                    b.Property<int>("DetayID")
                        .HasColumnType("int")
                        .HasColumnName("DetayID");

                    b.Property<int>("UrunID")
                        .HasColumnType("int")
                        .HasColumnName("UrunID");

                    b.HasKey("IcerirID")
                        .HasName("PK_Icerir_IcerirID");

                    b.HasIndex("DetayID");

                    b.HasIndex("UrunID");

                    b.ToTable("Icerir");
                });

            modelBuilder.Entity("alisveris.Models.Kargo", b =>
                {
                    b.Property<int>("KargoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KargoID"));

                    b.Property<string>("Durum")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Durum");

                    b.Property<string>("KargoSirketi")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("KargoSirketi");

                    b.Property<int>("SiparisID")
                        .HasColumnType("int");

                    b.Property<string>("TakipNo")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)")
                        .HasColumnName("TakipNo");

                    b.HasKey("KargoID")
                        .HasName("PK_Kargo_KargoID");

                    b.HasIndex("SiparisID")
                        .IsUnique();

                    b.HasIndex("TakipNo")
                        .IsUnique()
                        .HasDatabaseName("IX_Kargo_TakipNo");

                    b.ToTable("Kargo", t =>
                        {
                            t.HasCheckConstraint("CK_Kargo_TakipNo_Length", "LEN(TakipNo) = 6");
                        });
                });

            modelBuilder.Entity("alisveris.Models.Kullanici", b =>
                {
                    b.Property<int>("KullaniciID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KullaniciID"));

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Adres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EPosta")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sifre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Soyad")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("KullaniciID")
                        .HasName("PK_Kullanici_KullaniciID");

                    b.HasIndex("EPosta")
                        .IsUnique()
                        .HasDatabaseName("IX_Kullanici_EPosta");

                    b.ToTable("Kullanici", t =>
                        {
                            t.HasCheckConstraint("CK_Kullanici_Rol", "[Rol] IN ('Admin', 'Kullanıcı')");
                        });
                });

            modelBuilder.Entity("alisveris.Models.Siparis", b =>
                {
                    b.Property<int>("SiparisID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SiparisID"));

                    b.Property<int>("KullaniciID")
                        .HasColumnType("int");

                    b.Property<DateTime>("SiparisTarihi")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("TeslimTarihi")
                        .HasColumnType("datetime");

                    b.Property<decimal>("ToplamFiyat")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("SiparisID")
                        .HasName("PK_Siparis_SiparisID");

                    b.HasIndex("KullaniciID");

                    b.ToTable("Siparis");
                });

            modelBuilder.Entity("alisveris.Models.SiparisDetayi", b =>
                {
                    b.Property<int>("DetayID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DetayID"));

                    b.Property<decimal>("AraToplam")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("AraToplam");

                    b.Property<int>("Miktar")
                        .HasColumnType("int")
                        .HasColumnName("Miktar");

                    b.Property<int>("SiparisID")
                        .HasColumnType("int")
                        .HasColumnName("SiparisID");

                    b.HasKey("DetayID")
                        .HasName("PK_SiparisDetayi_DetayID");

                    b.HasIndex("SiparisID");

                    b.ToTable("SiparisDetaylari", t =>
                        {
                            t.HasCheckConstraint("CK_SiparisDetayi_AraToplam", "[AraToplam] >= 0");

                            t.HasCheckConstraint("CK_SiparisDetayi_Miktar", "[Miktar] > 0");
                        });
                });

            modelBuilder.Entity("alisveris.Models.Urun", b =>
                {
                    b.Property<int>("UrunID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UrunID"));

                    b.Property<string>("Aciklama")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("Aciklama");

                    b.Property<decimal>("Fiyat")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Fiyat");

                    b.Property<int>("StokAdedi")
                        .HasColumnType("int")
                        .HasColumnName("StokAdedi");

                    b.Property<string>("UrunAdi")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("UrunAdi");

                    b.HasKey("UrunID")
                        .HasName("PK_Urun_UrunID");

                    b.ToTable("Urunler", t =>
                        {
                            t.HasCheckConstraint("CK_Urun_Fiyat_Positive", "[Fiyat] >= 0");

                            t.HasCheckConstraint("CK_Urun_StokAdedi_Positive", "[StokAdedi] >= 0");
                        });
                });

            modelBuilder.Entity("Odeme", b =>
                {
                    b.HasOne("alisveris.Models.Kullanici", "Kullanici")
                        .WithMany("Odemeler")
                        .HasForeignKey("KullaniciID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Odeme_Kullanici_KullaniciID");

                    b.Navigation("Kullanici");
                });

            modelBuilder.Entity("alisveris.Models.Icerir", b =>
                {
                    b.HasOne("alisveris.Models.SiparisDetayi", "SiparisDetayi")
                        .WithMany("Icerir")
                        .HasForeignKey("DetayID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Icerir_SiparisDetayi_DetayID");

                    b.HasOne("alisveris.Models.Urun", "Urun")
                        .WithMany("Icerir")
                        .HasForeignKey("UrunID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Icerir_Urun_UrunID");

                    b.Navigation("SiparisDetayi");

                    b.Navigation("Urun");
                });

            modelBuilder.Entity("alisveris.Models.Kargo", b =>
                {
                    b.HasOne("alisveris.Models.Siparis", "Siparis")
                        .WithOne("Kargo")
                        .HasForeignKey("alisveris.Models.Kargo", "SiparisID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Kargo_Siparis_SiparisID");

                    b.Navigation("Siparis");
                });

            modelBuilder.Entity("alisveris.Models.Siparis", b =>
                {
                    b.HasOne("alisveris.Models.Kullanici", "Kullanici")
                        .WithMany("Siparisler")
                        .HasForeignKey("KullaniciID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Siparis_Kullanici_KullaniciID");

                    b.Navigation("Kullanici");
                });

            modelBuilder.Entity("alisveris.Models.SiparisDetayi", b =>
                {
                    b.HasOne("alisveris.Models.Siparis", "Siparis")
                        .WithMany("SiparisDetaylari")
                        .HasForeignKey("SiparisID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_SiparisDetayi_Siparis_SiparisID");

                    b.Navigation("Siparis");
                });

            modelBuilder.Entity("alisveris.Models.Kullanici", b =>
                {
                    b.Navigation("Odemeler");

                    b.Navigation("Siparisler");
                });

            modelBuilder.Entity("alisveris.Models.Siparis", b =>
                {
                    b.Navigation("Kargo")
                        .IsRequired();

                    b.Navigation("SiparisDetaylari");
                });

            modelBuilder.Entity("alisveris.Models.SiparisDetayi", b =>
                {
                    b.Navigation("Icerir");
                });

            modelBuilder.Entity("alisveris.Models.Urun", b =>
                {
                    b.Navigation("Icerir");
                });
#pragma warning restore 612, 618
        }
    }
}
