using NotUyg.Entity;

namespace NotUyg.Models
{
    public class AnketListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descirption { get; set; }
        public DateTime Time { get; set; }
        public ICollection<OptionViewModel> Options { get; set; }
        public bool HVoted { get; set; }
    }
}
