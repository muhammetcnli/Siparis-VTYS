using System.ComponentModel.DataAnnotations;
using alisveris.Models;

public class Odeme
{
    public int OdemeID { get; set; } // Primary Key
    public int KullaniciID { get; set; } // Foreign Key
    [Required]
    public string OdemeTuru { get; set; } // Kart, kripto, kapıdaÖdeme
    [Required]
    public DateTime OdemeTarihi { get; set; } // gelecek bir zaman olamaz
    [Required]
    public Boolean OdemeDurumu { get; set; } // 0, 1

    public Kullanici Kullanici { get; set; }
}
