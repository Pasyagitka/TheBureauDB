using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TheBureau.Models
{
    [Table("RequestTool")]
    public partial class RequestTool
    {
        public int id { get; set; }
        public int requestId { get; set; }
        public int toolId { get; set; }
        
        public virtual Tool Tool { get; set; }
        public virtual Request Request { get; set; }
    }
}
