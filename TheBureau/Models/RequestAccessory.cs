using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TheBureau.Models
{
    [Table("RequestAccessory")]
    public partial class RequestAccessory
    {
        public int id { get; set; }

        public int requestId { get; set; }

        [Required] 
        [StringLength(2)] 
        public string accessoryId { get; set; }

        public int quantity { get; set; }

        public virtual Accessory Accessory { get; set; }

        public virtual Request Request { get; set; }
    }
}