using ShopOnline.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models

{
    [Table("OrderEntry")]

    public class OrderEntry
    {
        [Key]
        public int OEId { get; set; }

        public int AccountId { get; set; }

        [DataType(DataType.Text)]
        public string DateOE { get; set; }

        [Column(TypeName = "Money")]
        public decimal TotalAmount { set; get; }
        public int OEStatus { get; set; } = 0;

    }
}
