using System.ComponentModel.DataAnnotations.Schema;

namespace NotUyg.Entity
{
    public class UserVote
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [ForeignKey(nameof(Poll))]
        public int PollId { get; set; }

        [ForeignKey(nameof(Option))]
        public int OptionId { get; set; }

        public DateTime Date { get; set; }

        public User User { get; set; }
        public  Poll Poll { get; set; }
        public  Option Option { get; set; } 
    };
}
