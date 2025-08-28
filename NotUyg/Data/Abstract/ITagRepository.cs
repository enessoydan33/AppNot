using NotUyg.Entity;

namespace NotUyg.Data.Abstract
{
    public interface ITagRepository
    {
        public IQueryable<Tag> Tag{ get; }
        public void Create(Tag tag);
    }
}
