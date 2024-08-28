using Azure;
using ShopOnline.Models;

namespace ShopOnline.Service
{
    public class ProductRepository : IProduct
    { 
        private readonly ShopContext _product;

        public ProductRepository(ShopContext product)
        {
            _product = product;
        }

        public List<Product> getProductAll()
        {
            var products = _product.Product.ToList();
            return products;
        }


        public Product? getProductById(int id)
        {
            var product = _product.Product.Find(id);
          
            return product;
        }

        public void AddProduct(Product product)
        {
            _product.Product.Add(product);
            _product.SaveChanges();
        }

       

        public List<Product> getList(int pageNumber, int pageSize)
            
        {
            var proList = _product.Product
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList(); ;
            return proList;
        }

        public void Update(Product old,Product pro)
        {
            old.ProductName = pro.ProductName;
            old.Description = pro.Description;
            old.Price = pro.Price;
            old.EntryPrice = pro.EntryPrice;
            old.CategoryId = pro.CategoryId;
            old.SupId = pro.SupId;
            old.Note = pro.Note;

            _product.SaveChanges();
        }

        public void Delete(Product product)
        {            
            _product.Product.Remove(product);
            _product.SaveChanges();
        }
    }
}
