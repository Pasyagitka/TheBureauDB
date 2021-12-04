using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TheBureau.Models;

namespace TheBureau.Repositories
{
    public class RequestEquipmentRepository : IRepository<RequestEquipment>
    {
        private Model _context = new Model();
        public IEnumerable<RequestEquipment> GetAll()
        {
            return _context.RequestEquipments;
        }

        public IEnumerable<RequestEquipment> GetAllByRequestId(int requestId)
        {
            return GetAll().Where(x => x.requestId == requestId);
        }
        public RequestEquipment Get(int id)
        {
            return _context.RequestEquipments.Find(id);
        }

        public void Add(RequestEquipment item)
        {
            _context.RequestEquipments.Add(item);
        }

        public void Update(RequestEquipment item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var requestequipment = _context.RequestEquipments.Find(id);
            if (requestequipment != null)
            {
                _context.RequestEquipments.Remove(requestequipment);
            }
        }

        public IEnumerable<RequestEquipment> FindByRequestId(int requestId)
        {
            return GetAll().Where(x => x.requestId == requestId);
        }

        public void DeleteByRequestId(int requestId)
        {
            var requests = GetAll().Where(x => x.requestId == requestId);
            foreach (var r in requests)
            {
                Delete(r.id);
            }
        }
        
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}