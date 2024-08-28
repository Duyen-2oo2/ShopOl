using ShopOnline.Models;

namespace ShopOnline.Service
{
    public interface IItemCart
    {
        void AddItemCart(int id, int poid, int sizeid, int quantity);
        ItemCart? CheckIcId(int id);
        void RemoveItemCart(ItemCart ic);
        void UpdateItemCart(ItemCart ico, ItemCart icn);
        List<ItemCart> GetListItemCart(int id);
    }
    public class ItemCartRepository : IItemCart
    {
        private readonly ShopContext _itemcart;

        public ItemCartRepository(ShopContext itemcart)
        {
            _itemcart = itemcart;
        }

        public void AddItemCart(int id,int poid, int sizeid, int quantity)
        {
            ItemCart itemcart = new ItemCart()
            {
                CartId = id,
                ProductId = poid,
                SizeId = sizeid,
                Quantity = quantity
            };
            _itemcart.Add(itemcart);
            _itemcart.SaveChanges();

        }

         public ItemCart? CheckIcId(int id)
        {
            var ic = _itemcart.ItemCart.SingleOrDefault(i => i.ICId == id);
            if (ic == null)
                return null;
            return ic;
        }

        void IItemCart.RemoveItemCart(ItemCart ic)
        {
            _itemcart.Remove(ic);
            _itemcart.SaveChanges();
        }

        void IItemCart.UpdateItemCart(ItemCart ico, ItemCart icn)
        {         
            ico.SizeId = icn.SizeId;
            ico.Quantity = icn.Quantity;
            _itemcart.SaveChanges();
        }

        public List<ItemCart> GetListItemCart(int id)
        {
            var listitem = _itemcart.ItemCart
                                    .Where(i => i.CartId == id)
                                    .ToList();
            if(listitem != null)
                return listitem;
            return [];
        }
    }
}
