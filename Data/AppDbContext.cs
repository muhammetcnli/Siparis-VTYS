using alisveris.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSet Definitions
    public DbSet<Kullanici> Kullanici { get; set; }
    public DbSet<Odeme> Odeme { get; set; }
    public DbSet<Siparis> Siparis { get; set; }
    public DbSet<Kargo> Kargo { get; set; }
    public DbSet<SiparisDetayi> SiparisDetaylari { get; set; }
    public DbSet<Icerir> Icerir { get; set; }
    public DbSet<Urun> Urunler { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    // Kullanıcı tablosu için kısıtlamalar
    modelBuilder.Entity<Kullanici>()
        .HasKey(k => k.KullaniciID)
        .HasName("PK_Kullanici_KullaniciID");

    modelBuilder.Entity<Kullanici>()
        .Property(k => k.Ad)
        .IsRequired()
        .HasMaxLength(50);
        

    modelBuilder.Entity<Kullanici>()
        .Property(k => k.Soyad)
        .IsRequired()
        .HasMaxLength(50);
        

    modelBuilder.Entity<Kullanici>()
        .Property(k => k.EPosta)
        .IsRequired()
        .HasMaxLength(100);
        

    modelBuilder.Entity<Kullanici>()
        .HasIndex(k => k.EPosta)
        .IsUnique()
        .HasName("IX_Kullanici_EPosta"); 

    modelBuilder.Entity<Kullanici>()
        .HasCheckConstraint("CK_Kullanici_Rol", "[Rol] IN ('Admin', 'Kullanıcı')");

    
        // Siparis tablosu


    modelBuilder.Entity<Siparis>()
        .HasKey(s => s.SiparisID)  // Primary Key
        .HasName("PK_Siparis_SiparisID");

    // Siparis tablosu için ToplamFiyat kısıtlaması
    modelBuilder.Entity<Siparis>()
        .Property(s => s.ToplamFiyat)
        .IsRequired() // ToplamFiyat zorunlu
        .HasColumnType("decimal(18,2)"); // Sayısal veri türü

    // SiparisTarihi için gelecek tarih kısıtlaması
    modelBuilder.Entity<Siparis>()
        .Property(s => s.SiparisTarihi)
        .IsRequired() // SiparisTarihi zorunlu
        .HasColumnType("datetime");

    // modelBuilder.Entity<Siparis>()
    //    .HasCheckConstraint("CK_Siparis_SiparisTarihi_GelecekTarih", "[SiparisTarihi] <= GETDATE()") // Gelecek tarih olamaz
    //    .HasName("CK_Siparis_SiparisTarihi_GelecekTarih");

    // TeslimTarihi kısıtlaması (sipariş tarihinden önce olamaz)
    modelBuilder.Entity<Siparis>()
        .Property(s => s.TeslimTarihi)
        .HasColumnType("datetime");

    // modelBuilder.Entity<Siparis>()
    //    .HasCheckConstraint("CK_Siparis_TeslimTarihi", "[TeslimTarihi] >= [SiparisTarihi] OR [TeslimTarihi] IS NULL") // Teslim tarihi sipariş tarihinden önce olamaz
    //    .HasName("CK_Siparis_TeslimTarihi");

    // KullaniciID Foreign Key ilişkisi
    modelBuilder.Entity<Siparis>()
        .HasOne(s => s.Kullanici)
        .WithMany(k => k.Siparisler)
        .HasForeignKey(s => s.KullaniciID)
        .HasConstraintName("FK_Siparis_Kullanici_KullaniciID");

    // Kargo ilişkisi 1-1
    modelBuilder.Entity<Siparis>()
        .HasOne(s => s.Kargo)
        .WithOne(k => k.Siparis)
        .HasForeignKey<Kargo>(k => k.SiparisID)
        .HasConstraintName("FK_Siparis_Kargo_SiparisID");

    // SiparisDetayi ilişkisi N-1
    modelBuilder.Entity<Siparis>()
        .HasMany(s => s.SiparisDetaylari)
        .WithOne(sd => sd.Siparis)
        .HasForeignKey(sd => sd.SiparisID)
        .HasConstraintName("FK_Siparis_SiparisDetayi_SiparisID");
    

    // odeme
    // Odeme tablosu için kısıtlamalar
    modelBuilder.Entity<Odeme>()
        .HasKey(o => o.OdemeID) // OdemeID Primary Key
        .HasName("PK_Odeme_OdemeID");

    modelBuilder.Entity<Odeme>()
        .Property(o => o.KullaniciID) // KullaniciID Foreign Key
        .IsRequired()
        .HasColumnName("KullaniciID");

    modelBuilder.Entity<Odeme>()
        .Property(o => o.OdemeTuru) // OdemeTuru Required ve MaxLength
        .IsRequired()
        .HasMaxLength(50)
        .HasColumnName("OdemeTuru");

    modelBuilder.Entity<Odeme>()
        .Property(o => o.OdemeTarihi) // OdemeTarihi için Gelecek tarih kısıtlaması
        .IsRequired()
        .HasColumnName("OdemeTarihi");

    modelBuilder.Entity<Odeme>()
        .Property(o => o.OdemeDurumu) // OdemeDurumu için 0 veya 1 kısıtlaması
        .IsRequired()
        .HasColumnName("OdemeDurumu");

    // Kullanıcı tablosuna Foreign Key kısıtlaması
    modelBuilder.Entity<Odeme>()
        .HasOne(o => o.Kullanici) // Odeme tablosunun Kullanici tablosuyla ilişkisi
        .WithMany(k => k.Odemeler)
        .HasForeignKey(o => o.KullaniciID)
        .HasConstraintName("FK_Odeme_Kullanici_KullaniciID");

    // OdemeTarihi kısıtlaması - Gelecek tarih olamaz
    modelBuilder.Entity<Odeme>()
        .HasCheckConstraint("CK_Odeme_OdemeTarihi", "[OdemeTarihi] <= GETDATE()"); // Gelecek tarih olamaz

    // OdemeDurumu kısıtlaması - 0 veya 1 olmalı
    modelBuilder.Entity<Odeme>()
        .HasCheckConstraint("CK_Odeme_OdemeDurumu", "[OdemeDurumu] IN (0, 1)"); // OdemeDurumu 0 veya 1 olmalı


    // sipariş detayı 

    // SiparisDetayi tablosu için kısıtlamalar
    modelBuilder.Entity<SiparisDetayi>()
        .HasKey(sd => sd.DetayID) // DetayID Primary Key
        .HasName("PK_SiparisDetayi_DetayID");

    modelBuilder.Entity<SiparisDetayi>()
        .Property(sd => sd.SiparisID) // SiparisID Foreign Key
        .IsRequired()
        .HasColumnName("SiparisID");

    modelBuilder.Entity<SiparisDetayi>()
        .Property(sd => sd.Miktar) // Miktar required, 0 olamaz
        .IsRequired()
        .HasColumnName("Miktar");

    modelBuilder.Entity<SiparisDetayi>()
        .Property(sd => sd.AraToplam) // AraToplam required, - olamaz
        .IsRequired()
        .HasColumnName("AraToplam");

    // Siparis tablosuna Foreign Key kısıtlaması
    modelBuilder.Entity<SiparisDetayi>()
        .HasOne(sd => sd.Siparis) // SiparisDetayi tablosunun Siparis tablosuyla ilişkisi
        .WithMany(s => s.SiparisDetaylari)
        .HasForeignKey(sd => sd.SiparisID)
        .HasConstraintName("FK_SiparisDetayi_Siparis_SiparisID");

    // Miktar kısıtlaması: 0 olamaz, negatif olamaz
    modelBuilder.Entity<SiparisDetayi>()
        .HasCheckConstraint("CK_SiparisDetayi_Miktar", "[Miktar] > 0");

    // AraToplam kısıtlaması: Negatif olamaz
    modelBuilder.Entity<SiparisDetayi>()
        .HasCheckConstraint("CK_SiparisDetayi_AraToplam", "[AraToplam] >= 0");

        // Içerir tablosu

        // Icerir tablosu için kısıtlamalar
    modelBuilder.Entity<Icerir>()
        .HasKey(i => i.IcerirID) // IcerirID Primary Key
        .HasName("PK_Icerir_IcerirID");

    modelBuilder.Entity<Icerir>()
        .Property(i => i.UrunID) // UrunID Foreign Key
        .IsRequired()
        .HasColumnName("UrunID");

    modelBuilder.Entity<Icerir>()
        .Property(i => i.DetayID) // DetayID Foreign Key
        .IsRequired()
        .HasColumnName("DetayID");

    // UrunID için Foreign Key kısıtlaması
    modelBuilder.Entity<Icerir>()
        .HasOne(i => i.Urun) // Icerir tablosunun Urun tablosuyla ilişkisi
        .WithMany(u => u.Icerir) // Urun tablosunda Icerirler koleksiyonu olacak
        .HasForeignKey(i => i.UrunID)
        .HasConstraintName("FK_Icerir_Urun_UrunID");

    // DetayID için Foreign Key kısıtlaması
    modelBuilder.Entity<Icerir>()
        .HasOne(i => i.SiparisDetayi) // Icerir tablosunun SiparisDetayi tablosuyla ilişkisi
        .WithMany(sd => sd.Icerir) // SiparisDetayi tablosunda Icerirler koleksiyonu olacak
        .HasForeignKey(i => i.DetayID)
        .HasConstraintName("FK_Icerir_SiparisDetayi_DetayID");


    //Urun tablosu

    // Urun tablosu için kısıtlamalar
    modelBuilder.Entity<Urun>()
        .HasKey(u => u.UrunID) // UrunID Primary Key
        .HasName("PK_Urun_UrunID");

    modelBuilder.Entity<Urun>()
        .Property(u => u.UrunAdi) // UrunAdi alanı gereklidir
        .IsRequired()
        .HasColumnName("UrunAdi")
        .HasMaxLength(255); // Maksimum uzunluk ekleyebilirsiniz

    modelBuilder.Entity<Urun>()
        .Property(u => u.Aciklama) // Aciklama alanı isteğe bağlı
        .HasMaxLength(500) // Maksimum uzunluk
        .HasColumnName("Aciklama");

    modelBuilder.Entity<Urun>()
        .Property(u => u.StokAdedi) // StokAdedi required ve negatif olamaz
        .IsRequired()
        .HasColumnName("StokAdedi");

    modelBuilder.Entity<Urun>()
        .Property(u => u.Fiyat) // Fiyat required ve negatif olamaz
        .IsRequired()
        .HasColumnName("Fiyat");

    modelBuilder.Entity<Urun>()
        .HasCheckConstraint("CK_Urun_StokAdedi_Positive", "[StokAdedi] >= 0") // StokAdedi negatif olamaz
        .HasCheckConstraint("CK_Urun_Fiyat_Positive", "[Fiyat] >= 0");


    // Kargo tablosu

    // Kargo tablosu için kısıtlamalar
    modelBuilder.Entity<Kargo>()
        .HasKey(k => k.KargoID) // KargoID Primary Key
        .HasName("PK_Kargo_KargoID");

    modelBuilder.Entity<Kargo>()
        .Property(k => k.TakipNo) // TakipNo unique olacak ve 6 basamaktan oluşacak
        .IsRequired() // Required kısıtlaması
        .HasMaxLength(6) // 6 basamadan oluşacak
        .HasColumnName("TakipNo");

    modelBuilder.Entity<Kargo>()
        .HasIndex(k => k.TakipNo) // TakipNo'nun benzersiz olmasını sağla
        .IsUnique()
        .HasName("IX_Kargo_TakipNo");

    modelBuilder.Entity<Kargo>()
        .Property(k => k.Durum) // Durum required
        .IsRequired()
        .HasColumnName("Durum")
        .HasMaxLength(50);

    modelBuilder.Entity<Kargo>()
        .Property(k => k.KargoSirketi) // KargoSirketi required
        .IsRequired()
        .HasColumnName("KargoSirketi")
        .HasMaxLength(50);

    // Foreign key ilişkisi
    modelBuilder.Entity<Kargo>()
        .HasOne(k => k.Siparis) // Kargo tablosu Siparis tablosuyla ilişkilendiriliyor
        .WithOne(s => s.Kargo) // Her sipariş bir kargo ile ilişkili
        .HasForeignKey<Kargo>(k => k.SiparisID) // SiparisID foreign key olarak tanımlanıyor
        .HasConstraintName("FK_Kargo_Siparis_SiparisID");

    // TakipNo için check constraint (6 basamaklı)
    modelBuilder.Entity<Kargo>()
        .HasCheckConstraint("CK_Kargo_TakipNo_Length", "LEN(TakipNo) = 6");

        base.OnModelCreating(modelBuilder);
    }
}
