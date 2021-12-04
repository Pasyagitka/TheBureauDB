using System.Collections.Generic;
using System.Linq;
using TheBureau.Enums;
using TheBureau.Models;

namespace TheBureau.Repositories
{
    public class ToolRepository
    {
        private Model _context = new Model();
        public IEnumerable<Tool> GetAll()
        {
            return _context.Tools;
        }
        public Tool Get(int id)
        {
            return _context.Tools.Find(id);
        }
        public IEnumerable<Tool> GetByStage(int stage)
        {
            return _context.Tools.Where(x => x.stage== stage);
        }
    }
}