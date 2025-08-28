using NotUyg.Entity;

namespace NotUyg.Data.Abstract
{
    public interface IUserVoteRepository
    {
        public IQueryable<UserVote> userVotes { get; }
        public void Create(UserVote userVote);


    }
}
