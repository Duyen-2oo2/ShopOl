using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("Supplier")]
    public class Supplier
    {
        [Key]                                  
        public int SupId { set; get; }

        [Required]                              
        [StringLength(50)]                      
        public string? SupName { set; get; }
    }
}
