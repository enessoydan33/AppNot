using NotUyg.Data.Abstract;
using NotUyg.Entity;

namespace NotUyg.Data.Concrete.EfCore
{
    public class AnketRepository : IAnketRepository
    {
        private readonly NotContext _context;
        public AnketRepository(NotContext context)
        {
            _context = context;
        }
        public IQueryable<Poll> polls => _context.Poll;

        public void Create(Poll poll)
        {
            _context.Poll.Add(poll);
           
            _context.SaveChanges();

        }

        public void DeletePoll(Poll poll)
        {
            _context.Poll.Remove(poll);
            _context.SaveChanges();


        }

        public void UpdatePoll(Poll poll)
        {
           var m= _context.Poll.Find(poll.Id);
            m.Description = poll.Description;
            m.Title = poll.Description;

            _context.SaveChanges();

        }
    }
}
