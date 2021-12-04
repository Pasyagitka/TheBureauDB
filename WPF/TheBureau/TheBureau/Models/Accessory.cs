using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBureau.Models
{
    [Table("Accessory")]
    public partial class Accessory
    {
        public int id { get; set; }

        [StringLength(15)]
        public string art { get; set; }

        [Required]
        [StringLength(2)]
        public string equipmentId { get; set; }

        [Required]
        [StringLength(150)]
        public string name { get; set; }

        public decimal price { get; set; }

        public virtual Equipment Equipment { get; set; }
    }
}
