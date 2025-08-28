using System.ComponentModel.DataAnnotations.Schema;

namespace NotUyg.Entity
{
    public class Not
    {
        public Not()
        {
            Tags = new List<Tag>();
        }

        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public DateTime Tarih { get; set; }
        public string acıklama { get; set; }
        public bool Durum {  get; set; }
        public string Baslık {  get; set; }
        public List<Tag> Tags { get; set; }
        public User User { get; set; }



    }
}
