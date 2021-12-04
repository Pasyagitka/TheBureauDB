using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBureau.Models
{
    [Table("Company")]
    public partial class Company
    {
        [Key]
        [StringLength(255)]
        public string email { get; set; }

        [StringLength(70)]
        public string password { get; set; }
    }
}
