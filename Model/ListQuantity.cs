using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("ListQuantity")]
    public class ListQuantity
    {
        [Key]
        public int QuantityId { get; set; }

        public int ProductId { get; set; }

        public int SizeId  { get; set; }

        public int Quantity { get; set; } 
    }
}
