using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBureau.Models
{
    [Table("Employee")]
    public partial class Employee
    {
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

        [StringLength(255)]
        public string email { get; set; }

        public decimal? contactNumber { get; set; }

        public int? brigadeId { get; set; }

        public virtual Brigade Brigade { get; set; }
    }
}
