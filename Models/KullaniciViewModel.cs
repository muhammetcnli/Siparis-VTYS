namespace alisveris.Models
{
    public class KullaniciViewModel
    {
        public string SearchQuery { get; set; } // Ad'a göre arama sorgusu
        public string SortOrder { get; set; }    // Sıralama için kullanılan parametre (Ad, Soyad, Rol)
        public List<Kullanici> Kullanicilar { get; set; }  // Kullanıcı listesi
    }
}