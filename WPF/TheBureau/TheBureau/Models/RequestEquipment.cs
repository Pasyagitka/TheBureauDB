using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBureau.Models
{
    [Table("RequestEquipment")]
    public partial class RequestEquipment
    {
        public int id { get; set; }

        public int requestId { get; set; }

        [Required]
        [StringLength(2)]
        public string equipmentId { get; set; }

        public int? quantity { get; set; }

        public virtual Equipment Equipment { get; set; }

        public virtual Request Request { get; set; }
    }
}
