using System.ComponentModel.DataAnnotations;

namespace alisveris.Models
{
    public class RegistrationViewModel{
        
        public int KullaniciID { get; set; } // Primary Key (Genellikle veritabanı tarafından otomatik olarak atanır)
        
        [Required(ErrorMessage = "Ad alanı boş bırakılamaz.")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz.")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "E-posta alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        public string EPosta { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }
        public string Rol { get; set; }

        [Required(ErrorMessage = "Adres alanı boş bırakılamaz.")]
        public string Adres { get; set; }
    }
}