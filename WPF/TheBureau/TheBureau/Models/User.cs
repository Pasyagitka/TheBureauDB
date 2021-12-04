using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBureau.Models
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            Brigades = new HashSet<Brigade>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(20)]
        public string login { get; set; }

        [Required]
        [StringLength(70)]
        public string password { get; set; }

        public int role { get; set; }

        public virtual ICollection<Brigade> Brigades { get; set; }

        public virtual Role Role1 { get; set; }
    }
}
