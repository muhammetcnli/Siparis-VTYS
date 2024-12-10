using System.ComponentModel.DataAnnotations;
namespace alisveris.Models;

public class Kullanici
{
    [Key]
    public int KullaniciID { get; set; } // Primary Key
    [Required]
    public string Ad { get; set; }
    [Required]
    public string Soyad { get; set; }
    [Required]

    [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
    
    public string EPosta { get; set; } // e-posta için enum ile özel format
    
    [Required]
    public string Sifre { get; set; } 
    
    [Required]
    public string Rol { get; set; } // admin, kullanıcı
    [Required]
    public string Adres { get; set; }

    public ICollection<Odeme> Odemeler { get; set; } // N-1 ilişki
    public ICollection<Siparis> Siparisler { get; set; } // N-1 ilişki
}
