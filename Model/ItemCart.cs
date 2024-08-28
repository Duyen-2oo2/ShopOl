using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("ItemCart")]
    public class ItemCart
    {
        [Key]
        public int ICId { get; set; }

        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int SizeId { get; set; }

        public int Quantity { get; set; }
    }
}
