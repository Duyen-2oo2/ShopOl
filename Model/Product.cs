using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using ShopOnline.Models;

namespace ShopOnline.Models
{
    [Table("Product")]                         
    public class Product
    {
        [Key]                                  
        public int ProductId { set; get; }

        [Required]                              
        [StringLength(50)]                      
        public string ProductName { set; get; }

        [Column(TypeName = "ntext")]
        public string Description { set; get; } 

        [Column(TypeName = "Money")]                          
        public decimal Price { set; get; }

        [Column(TypeName = "Money")]                      
        public decimal EntryPrice { set; get; }

        [Column(TypeName = "Int")]              

        public int CategoryId { set; get; }

        public int SupId { set; get; }
        public string Note { set; get; } = string.Empty;

    }
}
