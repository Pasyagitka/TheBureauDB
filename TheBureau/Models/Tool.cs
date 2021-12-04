using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBureau.Models
{
    [Table("Tool")]
    public partial class Tool
    {
        public int id { get; set; }

        [Required]
        //[StringLength(50)]
        public string name { get; set; }

        public int stage { get; set; }
    }
}
