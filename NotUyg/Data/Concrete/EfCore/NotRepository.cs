using Microsoft.EntityFrameworkCore;
using NotUyg.Data.Abstract;
using NotUyg.Entity;

namespace NotUyg.Data.Concrete.EfCore
{
    public class NotRepository : INotRepository
    {
        private readonly NotContext _context;
        public NotRepository(NotContext context)
        {
            _context = context;
        }
        public IQueryable<Not> Nots => _context.Not;

        public void Create(Not not)
        {
            _context.Not.Add(not);
            _context.SaveChanges();
        }

        public void UpdateNot(Not not)
        {
            var m=_context.Not.Find(not.Id); 
            m.Durum = not.Durum;
            m.Baslık = not.Baslık;
            m.acıklama = not.acıklama;
          m.Tags = not.Tags;
            _context.SaveChanges();
        }


        
        public void DeleteNot(Not not )
        {
                _context.Not.Remove(not);
                _context.SaveChanges();
            
        }

        public void TagClear(Not not)
        {
            var existing = _context.Not.Include(n => n.Tags).FirstOrDefault(n => n.Id == not.Id);
            existing.Tags.Clear();
            _context.SaveChanges();
        }

    }
}
