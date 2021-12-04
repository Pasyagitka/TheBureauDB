using System.Collections.Generic;
using System.Linq;
using TheBureau.Models;

namespace TheBureau.Repositories
{
    public class EquipmentRepository
    {
        private Model _context = new Model();
        public IEnumerable<Equipment> GetAll()
        {
            return _context.Equipments;
        }
        public Equipment Get(int id)
        {
            return _context.Equipments.Find(id);
        }
        public IEnumerable<Equipment> GetByID(string id)
        {
            return _context.Equipments.Where(x => x.id == id);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}