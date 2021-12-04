using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using TheBureau.Models;

namespace TheBureau.Repositories
{
    public class RequestRepository
    {
        private Model _context = new();
        public IEnumerable<Request> GetAll()
        {
            return _context.Requests;
        }
        public Request Get(int id)
        {
            return _context.Requests.Find(id);
        }
        public void Add(Request request)
        {
            _context.Requests.Add(request);
        }

        public int GetRedRequestsCount()
        {
            return _context.Requests.Count(x => x.status == 1);
        }
        public int GetYellowRequestsCount()
        {
            return _context.Requests.Count(x => x.status == 2);
        }
        public int GetGreenRequestsCount()
        {
            return _context.Requests.Count(x => x.status == 3);
        }

        public IEnumerable<Request> GetToDoRequests()
        {
            return _context.Requests.Where(x => x.status ==1 || x.status == 2);
        }
        public IEnumerable<Request> GetRequestsByBrigadeId(int id)
        {
            return _context.Requests.Where(x => x.brigadeId == id);
        }

        public IEnumerable<Request> GetToDoRequestsForBrigade()
        {
            return _context.Requests.Where(x => x.status ==1 || x.status == 2);
        }

        public void Update(Request forUpdate)
        {
            _context.Entry(forUpdate).State = EntityState.Modified;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var request = _context.Requests.Find(id);
            if (request != null)
            {
                _context.Requests.Remove(request);
            }
        }

        public IEnumerable<Request> FindByClientId(int clientId)
        {
            return _context.Requests.Where(x =>x.clientId == clientId);
        }

        public IEnumerable<Request> GetRequestsBySurnameOrEmail(string criteria)
        {
            return _context.Requests.Where(x => 
                x.Client.surname.ToLower().Equals(criteria.ToLower()) || 
                x.Client.email.ToLower().Equals(criteria.ToLower())
                );
        }
        public IEnumerable<Request> FindRequestsForBrigadeByCriteria(string criteria, int brigadeId)
        {
            return GetAll().Where(x => (x.Address.street.ToLower().Contains(criteria.ToLower()) ||
                                                x.Client.surname.ToLower().Contains(criteria.ToLower()) || 
                                                x.id.ToString().Contains(criteria)) 
                                       && x.brigadeId == brigadeId
            );
        }

        // public void DeleteRequestsOfClient(int clientId)
        // {
        //     var clientRequests = FindByClientId(clientId);
        //     List<int> addresses = new();
        //     List<int> requestsid = new();
        //     foreach (var request in clientRequests)
        //     {
        //         addresses.Add(request.addressId);
        //         requestsid.Add(request.id);
        //         Delete(request.id);
        //     }
        //     foreach (var id in addresses)
        //     {
        //         var address = _context.Addresses.Find(id);
        //         if (address != null)
        //         {
        //             _context.Addresses.Remove(address);
        //         }
        //     }
        //     foreach (var id in requestsid)
        //     {
        //         var equipments = _context.RequestEquipments.Where(x=>x.requestId == id);
        //         foreach (var e in equipments)
        //         {
        //             _context.RequestEquipments.Remove(e);
        //         }
        //     }
        // }

        public int RequestForBrigadeForCertainDay(int currentBridageId, DayOfWeek currentDayOfWeek)
        {
            var calendar = new GregorianCalendar();
            //текущий порядковый номер недели в году
            var currentWeek = new GregorianCalendar().GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            int count = 0;
            //для этой бригады
            foreach (var request in _context.Requests.Where(x=>x.brigadeId == currentBridageId))
            {
                if (//в заданный день недели
                    request.mountingDate.DayOfWeek == currentDayOfWeek &&
                    //если это этой текущей неделе
                    calendar.GetWeekOfYear(request.mountingDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) == currentWeek &&
                    //и если заявка еще не выполнена
                    (request.status is 1 or 2))
                {
                    count++;
                }
            }
            return count;
        }
    }
}