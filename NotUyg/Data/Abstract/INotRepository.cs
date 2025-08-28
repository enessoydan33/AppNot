using NotUyg.Entity;

namespace NotUyg.Data.Abstract
{
    public interface INotRepository
    {
        public IQueryable<Not> Nots { get;}
        public void Create(Not not);

        public void UpdateNot(Not not);

        public void DeleteNot(Not not);
        public void TagClear(Not not);
    }
}
