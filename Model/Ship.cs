using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models
{
    [Table("Ship")]

    public class Ship
    {
        [Key]
        public int ShipId { get; set; }

        [Required]
        [StringLength(50)]
        public string ShipName { get; set; } = string.Empty;

        [Column(TypeName = "Money")]
        public decimal ShipPrice { get; set; }
    }
}
