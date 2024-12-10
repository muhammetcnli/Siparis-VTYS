using System.ComponentModel.DataAnnotations;

namespace alisveris.Models
{
    public class Icerir
    {

        public int IcerirID { get; set; } // Primary Key
        public int UrunID { get; set; } // Foreign Key
        public int DetayID { get; set; } // Foreign Key

        public SiparisDetayi SiparisDetayi { get; set; }
        public Urun Urun { get; set; }
    }
}