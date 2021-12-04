using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBureau.Models
{
    [Table("Role")]
    public partial class Role
    { 
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int id { get; set; }

        [Column("role")]
        public int role1 { get; set; }

        [Required]
        [StringLength(10)]
        public string name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
