using System;
using System.Collections.Generic;
using System.Text;

namespace TheBureau.Models
{
    public partial class Schedule
    {
        public int id { get; set; }

        public int employeeId { get; set; }

        public int requestId { get; set; }
        
        public bool attend { get; set; }
    }
}
