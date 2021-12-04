using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBureau.Models
{
    [Table("Request")]
    public partial class Request
    {
        public Request()
        {
            RequestEquipments = new HashSet<RequestEquipment>();
        }

        public int id { get; set; }

        public int clientId { get; set; }

        public int addressId { get; set; }

        public int stage { get; set; }

        public int status { get; set; }

        [Column(TypeName = "date")]
        public DateTime mountingDate { get; set; }

        [StringLength(200)]
        public string comment { get; set; }

        public int? brigadeId { get; set; }

        public virtual Address Address { get; set; }

        public virtual Brigade Brigade { get; set; }

        public virtual Client Client { get; set; }

        public virtual ICollection<RequestEquipment> RequestEquipments { get; set; }
    }
}
