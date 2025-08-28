using NotUyg.Entity;

namespace NotUyg.Models
{
    public class AnketKayit
    {
        public string Title { get; set; }
        public List<string> OptName { get; set; } = new();
        public string Description { get; set; }
       
    }
}
