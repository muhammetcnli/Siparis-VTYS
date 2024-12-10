using System.ComponentModel.DataAnnotations;
namespace alisveris.Models
{
    public class Urun
    {
        public int UrunID { get; set; } // Primary Key
        [Required]
        public string UrunAdi { get; set; }
        [Required]
        public string Aciklama { get; set; }
        [Required]
        public int StokAdedi { get; set; } // - olamaz
        [Required]
        public decimal Fiyat { get; set; } // - olamaz

        public virtual ICollection<Icerir> Icerir { get; set; } // 1-N ilişki
    }
}