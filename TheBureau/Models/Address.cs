using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBureau.Models
{
    [Table("Address")]
    public partial class Address
    {
        public Address()
        {
            Requests = new HashSet<Request>();
        }

        public int id { get; set; }

        [StringLength(30)]
        public string country { get; set; }

        [Required]
        [StringLength(30)]
        public string city { get; set; }

        [Required]
        [StringLength(30)]
        public string street { get; set; }

        public int house { get; set; }

        [StringLength(10)]
        public string corpus { get; set; }

        public int? flat { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}
