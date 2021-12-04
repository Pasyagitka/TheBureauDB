using System.Collections.Generic;
using System.Data.Entity;

namespace TheBureau.Repositories
{
    interface IRepository<T> where T : class
    { 
        IEnumerable<T> GetAll();
        T Get(int id);
        public void Update(T item);
        void Add(T item);
        void Delete(int id);
    }
}