using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TheBureau.Models;
using TheBureau.Services;

namespace TheBureau.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private Model _context = new Model();
        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }
        public User Get(int id)
        {
            return _context.Users.Find(id);
        }
        
        public User Login(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.login == login);
            if (user == null) return null;
            return PasswordHash.ValidatePassword(password, user.password) ? user : null;
        }

        public void Update(User item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Add(User item)
        {
            _context.Users.Add(item);
        }
        
        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}