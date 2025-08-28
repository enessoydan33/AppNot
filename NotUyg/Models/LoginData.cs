using System.ComponentModel.DataAnnotations;

namespace NotUyg.Models
{
    public class LoginData
    {
        [EmailAddress]
        [Required]

        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(5)]
        
        public string Password { get; set; }
    }
}
