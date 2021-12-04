using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBureau.Models
{
    [Table("Brigade")]
    public partial class Brigade
    {
        public Brigade()
        {
            Employees = new HashSet<Employee>();
            Requests = new HashSet<Request>();
        }

        public int id { get; set; }

        public int? userId { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<Request> Requests { get; set; }

        public virtual User User { get; set; }
    }
}
