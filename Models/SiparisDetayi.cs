using System.ComponentModel.DataAnnotations;

namespace alisveris.Models
{
    public class SiparisDetayi
    {
        public int DetayID { get; set; } // Primary Key
        public int SiparisID { get; set; } // Foreign Key

        [Required]
        public int Miktar { get; set; } // - olamaz, 0 olamaz
        [Required]
        public decimal AraToplam { get; set; } // - olamaz, 0 olabilir

        public virtual Siparis Siparis { get; set; } // N-1 ilişki
        public virtual ICollection<Icerir> Icerir { get; set; } // 1-N ilişki
    }
}