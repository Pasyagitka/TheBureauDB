using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBureau.Models
{
    [Table("Client")]
    public partial class Client
    {

        public Client()
        {
            Requests = new HashSet<Request>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(20)]
        public string firstname { get; set; }

        [Required]
        [StringLength(20)]
        public string patronymic { get; set; }

        [Required]
        [StringLength(20)]
        public string surname { get; set; }

        [Required]
        [StringLength(255)]
        public string email { get; set; }

        public decimal contactNumber { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}
