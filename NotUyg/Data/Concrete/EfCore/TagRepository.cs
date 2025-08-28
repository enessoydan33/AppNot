using NotUyg.Data.Abstract;
using NotUyg.Entity;

namespace NotUyg.Data.Concrete.EfCore
{
    public class TagRepository : ITagRepository
    {
        private readonly NotContext _context;
        public TagRepository(NotContext context)
        {
            _context = context;
        }
        public IQueryable<Tag> Tag => _context.Tag;

        public void Create(Tag tag)
        {
           _context.Tag.Add(tag);
            _context.SaveChanges();
        }
    }
}
