using NotUyg.Entity;

namespace NotUyg.Models
{
    public class NotUpdateData
    {
        public int Id { get; set; }

        public string aciklama { get; set; }

        public string Baslik { get; set; }

        public List<int> Tags { get; set; }

        public bool Durum { get; set; }
    }
}
