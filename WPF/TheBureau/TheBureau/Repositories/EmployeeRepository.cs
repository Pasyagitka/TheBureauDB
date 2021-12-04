using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TheBureau.Models;

namespace TheBureau.Repositories
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private Model _context = new Model();
        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees;
        }
        public Employee Get(int id)
        {
            return _context.Employees.Find(id);
        }
        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
        }
        public void Update(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Employee> FindEmployeesByCriteria(string criteria)
        {
            return _context.Employees.Where(x => x.firstname.ToLower().Contains(criteria.ToLower())
                                       || x.surname.ToLower().Contains(criteria.ToLower()) 
                                       || x.patronymic.ToLower().Contains(criteria.ToLower())
                                       || x.email.ToLower().Contains(criteria.ToLower())
                                       || x.contactNumber.ToString().Contains(criteria)
                                       || x.brigadeId.ToString().Contains(criteria)
                                       || x.id.ToString().Contains(criteria));
        }
    }
}