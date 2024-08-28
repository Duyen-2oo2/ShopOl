using ShopOnline.Models;

namespace ShopOnline.Service
{
    public interface IProduct
    {
        List<Product> getProductAll();

        List<Product> getList(int pageNumber, int pageSize);

       
        Product? getProductById(int id);

        void AddProduct(Product product);
        void Update(Product proOld, Product proNew);

        void Delete(Product product);
    
    }

}
