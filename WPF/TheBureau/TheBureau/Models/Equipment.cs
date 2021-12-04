using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBureau.Models
{
    [Table("Equipment")]
    public partial class Equipment
    {
        public Equipment()
        {
            Accessories = new HashSet<Accessory>();
            RequestEquipments = new HashSet<RequestEquipment>();
        }

        [StringLength(2)]
        public string id { get; set; }

        [Required]
        [StringLength(30)]
        public string type { get; set; }

        [Required]
        [StringLength(5)]
        public string mounting { get; set; }

        public virtual ICollection<Accessory> Accessories { get; set; }

        public virtual ICollection<RequestEquipment> RequestEquipments { get; set; }
    }
}
