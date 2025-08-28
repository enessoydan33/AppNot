using Microsoft.AspNetCore.Identity;

namespace NotUyg.Entity
{
    public class User:IdentityUser
    {
        public List<Not> Nots { get; set; }
    }
}
