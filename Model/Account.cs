using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Models
{

    [Table("Account")]
    public class Account
    {
        [Key]
        public int AccountId { set; get; }

        [Required]
        [StringLength(50)]
        public string UserName { set; get; }

        [Required]
        [StringLength(60)]
        [Column(TypeName = "varchar")]
        public string Password { set; get; }

        [StringLength(50)]
        public string FullName { set; get; } = string.Empty;

        [StringLength(10)]
        public string Sex { set; get; } = string.Empty;


        [StringLength(10)]
        public string Address { set; get; } = string.Empty;


        public int PhoneNumber { set; get; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { set; get; } = string.Empty;

        

    }
}
