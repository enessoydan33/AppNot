using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NotUyg.Models
{
    public class KayitData
    {
        public string Isim { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(100, ErrorMessage = "Maximum karakter sayısı 100 dür")]
        [MinLength(5,ErrorMessage ="Minimum karakter sayısı 5 dir")]
    
        public string Password { get; set; }
        public string Soyad { get; set; }






    }
}
