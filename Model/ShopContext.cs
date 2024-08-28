using ShopOnline.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using ShopOnline.Model;
namespace ShopOnline.Models
{
    public class ShopContext : DbContext
    {
        //protected string connect_str = @"Data Source=WIN-QN0GTA6NDUP;
        //                                 Initial Catalog=ShopDB;
        //                                 Integrated Security=True;
        //                                 Encrypt=False";
        public ShopContext(DbContextOptions<ShopContext> options) : base(options) 
        {

        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Product> Product{ set; get; }     
        public DbSet<Size> Size{ set; get; }
        public DbSet<ListQuantity> ListQuantity { set; get; }
        public DbSet<Category> Category { set; get; }          
        public DbSet<Account> Account { set; get; }
        public DbSet<ItemCart> ItemCart { set; get; }
        public DbSet<ItemOrder> ItemOrder { set; get; }
        public DbSet<ItemOrderEntry> ItemOrderEntry { set; get; }
        public DbSet<Cart> Cart { set; get; }
        public DbSet<Order> Order { set; get; }
        public DbSet<OrderEntry> OrderEntry { set; get; }
        public DbSet<Payment> Payment { set; get; }
        public DbSet<Ship> Ship { set; get; }
        public DbSet<Supplier> Supplier { set; get; }

        
    }
}
