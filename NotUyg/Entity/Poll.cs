namespace NotUyg.Entity
{
    public class Poll
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public ICollection<Option> Options { get; set; }
        public User user { get; set; }

    }
}
