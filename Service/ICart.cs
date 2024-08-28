using ShopOnline.Models;

namespace ShopOnline.Service
{
    public interface ICart
    {
        public interface ICart
        {

            void AddCart(int accId);
            bool DeleteProductSize(int cartId);
            int GetIdCart(int proid);

        }

        public class CartRepository : ICart
        {
            private readonly ShopContext _cart;

            public CartRepository(ShopContext cart)
            {
                _cart = cart;
            }

            public void AddCart(int accId)
            {
                var cart = new Cart()
                {
                    AccountId = accId
                };
                _cart.Add(cart);
                _cart.SaveChanges();
            }

            bool ICart.DeleteProductSize(int cartId)
            {
                var c = _cart.Cart.Find(cartId);
                if (c == null)
                    return false;
                _cart.Remove(c);
                _cart.SaveChanges();
                return true;
            }
            public int GetIdCart(int accid)
            {
                var id = _cart.Cart
                              .Where(i => i.AccountId == accid)
                              .Select(i => i.CartId)
                              .SingleOrDefault();
                return id;
            }
        }
    }
    }
