using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { set; get; }

        [Required]
        [StringLength(50)]
        public string CategoryName { set; get; } = string.Empty;
    }
}
