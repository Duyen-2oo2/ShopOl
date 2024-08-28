using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentName { get; set; }
    }
}
