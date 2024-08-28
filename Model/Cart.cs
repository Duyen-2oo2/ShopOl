using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public int  AccountId { get; set; }
    }
}
