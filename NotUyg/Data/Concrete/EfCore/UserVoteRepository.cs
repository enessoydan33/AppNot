using NotUyg.Data.Abstract;
using NotUyg.Entity;

namespace NotUyg.Data.Concrete.EfCore
{
    public class UserVoteRepository : IUserVoteRepository
    {
        private readonly NotContext _context;
        public UserVoteRepository(NotContext context)
        {
            _context = context;
        }
        public IQueryable<UserVote> userVotes => _context.UserVote;

        public void Create(UserVote userVote)
        {
            _context.UserVote.Add(userVote);
            _context.SaveChanges();
        }
    }
}
