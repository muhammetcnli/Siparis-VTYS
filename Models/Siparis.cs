using System.ComponentModel.DataAnnotations;

namespace alisveris.Models
{
    public class Siparis
    {
        public int SiparisID { get; set; } // Primary Key
        public int KullaniciID { get; set; } // Foreign Key
        [Required]
        public decimal ToplamFiyat { get; set; } // - olamaz
        [Required]
        public DateTime SiparisTarihi { get; set; } // gelecek tarih olamaz
        [Required]
        public DateTime TeslimTarihi { get; set; } // sipariş tarihinden önce veya aynı gün olamaz


        public virtual Kullanici Kullanici { get; set; } // N-1 ilişki
        public virtual  Kargo Kargo { get; set; } // 1-1 ilişki
        public virtual  ICollection<SiparisDetayi> SiparisDetaylari { get; set; } // N-1 ilişki
    }
}
