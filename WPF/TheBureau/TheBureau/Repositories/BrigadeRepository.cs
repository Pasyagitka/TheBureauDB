using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TheBureau.Models;

namespace TheBureau.Repositories
{
    public class BrigadeRepository : IRepository<Brigade>
    {
        private Model _context = new Model();
        public IEnumerable<Brigade> GetAll()
        {
            return _context.Brigades;
        }
        public Brigade Get(int id)
        {
            return _context.Brigades.Find(id);
        }
        public void Add(Brigade brigade)
        {
            _context.Brigades.Add(brigade);
        }
        public void Update(Brigade brigade)
        {
            _context.Entry(brigade).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var brigade = _context.Brigades.Find(id);
            if (brigade != null)
            {
                _context.Brigades.Remove(brigade);
            }
        }
        public IEnumerable<string> GetBrigadesId()
        {
            return _context.Brigades.Select(x=>x.id.ToString());
        }
        
    }
}