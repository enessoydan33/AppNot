using NotUyg.Entity;

namespace NotUyg.Data.Abstract
{
    public interface IAnketRepository
    {
        public IQueryable<Poll> polls { get; }
        public void Create(Poll poll);

        public void UpdatePoll(Poll poll);

        public void DeletePoll(Poll poll);




    }
}
