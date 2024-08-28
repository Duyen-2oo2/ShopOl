
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models
{

    [Table("ItemOrder")]
    public class ItemOrder
    {
        [Key]
        public int IOId { get; set; }

        public int OId { get; set; }

        public int ProdductId { get; set; }

        public int SizeId { get; set; }

        public int Quantity { get; set; } = 0;      
    }
}
