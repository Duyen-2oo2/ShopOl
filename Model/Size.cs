using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("Size")]
    public class Size
    {
        [Key]
        public int SizeId { get; set; }

        [Required]      
        public string SizeName { get; set; } = string.Empty;

    }
}
