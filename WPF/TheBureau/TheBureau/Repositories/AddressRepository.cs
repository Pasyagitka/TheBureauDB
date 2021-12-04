using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TheBureau.Models;

namespace TheBureau.Repositories
{
    public class AddressRepository : IRepository<Address>
    {
        private Model _context = new Model();
        public IEnumerable<Address> GetAll()
        {
           return _context.Addresses;
        }

        public Address Get(int id)
        {
            return _context.Addresses.Find(id);
        }

        public void Add(Address item)
        {
            _context.Addresses.Add(item);
        }
        
        public void Update(Address item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var address = _context.Addresses.Find(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public bool IsDuplicateAddress(string city, string street, int house, string corpus, int flat)
        {
            var count = _context.Addresses.Count(x =>
                x.city.ToLower().Equals(city.ToLower()) && 
                x.street.Equals(street) && 
                x.house.ToString().Equals(house.ToString()) &&
                x.corpus.ToLower().Equals(corpus.ToLower()) &&
                x.flat.ToString().Equals(flat.ToString())
                );
            return count == 0;
        }

        public Address FindAddress(string city, string street, int house, string corpus, int flat)
        {
            return _context.Addresses.FirstOrDefault(x => x.city.ToLower().Equals(city.ToLower()) &&
                  x.street.ToLower().Equals(street.ToLower()) &&
                  x.house.ToString().Equals(house.ToString()) &&
                  x.corpus.ToLower().Equals(corpus.ToLower()) &&
                  x.flat.ToString().Equals(flat.ToString())
            );
        }
    }
}