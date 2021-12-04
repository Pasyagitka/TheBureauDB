using System;
using System.Collections.Generic;
using System.Linq;
using TheBureau.Models;

namespace TheBureau.Repositories
{
    public class AccessoryRepository 
    {
        private Model _context = new Model();
        public IEnumerable<Accessory> GetAll()
        {
            return _context.Accessories;
        }
        public Accessory Get(int id)
        {
            return _context.Accessories.Find(id);
        }

        public IEnumerable<Accessory> GetByEquipmentId(string id)
        {
            return _context.Accessories.Where(x => x.Equipment.id == id);
        }

        public decimal TotalPrice(IEnumerable<Accessory> accessories)
        {
            decimal totalPrice = 0;
            foreach (var accessory in accessories)
            {
                totalPrice += accessory.price;
            }
            return totalPrice;
        }
        
        public IEnumerable<Accessory> GetAccessories(IEnumerable<RequestEquipment> equipment)
        {
            List<Accessory> result = new List<Accessory>();
            foreach (var eq in equipment)
            {
                for (int i = 0; i < eq.quantity; i++)
                {
                    result.AddRange(_context.Accessories.Where(x => x.equipmentId == eq.equipmentId));
                }
            }
            return result.OrderByDescending(x=>x.price);
        }
    }
}