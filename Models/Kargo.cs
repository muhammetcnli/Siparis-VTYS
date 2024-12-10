using System.ComponentModel.DataAnnotations;

namespace alisveris.Models
{
    public class Kargo
    {
        public int KargoID { get; set; } // Primary Key

        [Required(ErrorMessage = "Sipariş ID gereklidir.")]
        public int SiparisID { get; set; } // Foreign Key
        [Required(ErrorMessage = "Takip numarası gereklidir.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Takip numarası 6 haneli olmalıdır.")]
        public string TakipNo { get; set; } // 6 basamalı bir takip nosu, sistem otomatik atayacak, aynı takip no kullanılmıyor olmalı, unique
        [Required(ErrorMessage = "Durum numarası gereklidir.")]
        public string Durum { get; set; } // sipariş alındı, hazırlanıyor, dağıtımda, teslim edildi, teslim edilemedi
        [Required(ErrorMessage = "Kargo şirketi gereklidir.")]
        public string KargoSirketi { get; set; } // a, b, c kargo

        public virtual Siparis Siparis { get; set; }
    }
}
