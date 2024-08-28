using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopOnline.Models;


namespace ShopOnline.Models
{
    [Table("Order")]

    public class Order
    {
        [Key]
        public int OId { get; set; }

        public int AccountId { get; set; }

        public int PaymentId { get; set; }

        [DataType(DataType.Text)]
        public string OrderDate { get; set; }

        [Column(TypeName = "Money")]
        public decimal TotalAmount { get; set; } = 0;

        public int OrderStatus { get; set; } = 0;
        
        public int ShipId { get; set; }


    }
}
