using ShopOnline.Models;

namespace ShopOnline.Service
{
    public interface IOrder
    {
        int AddOrder(int accid);
        bool AddItemOrder(int id, ItemOrder ito);
        Order? CheckOId(int id);
        bool CheckQuantityOrder(int oldQ, int newQ);
        List<Order> GetListOrder(int id);
        void UpOrder(Order od,int shipid,int payid);
        void UpOrderStatus(Order od, int status);
        void UpTotal(List<ItemOrder> litod, Order od);
        void UpQuantity(List<ItemOrder> litod, Order od);
        List<ItemOrder> GetListItemOrder(int oid);
        void ItemCartToItemOrder(List<ItemCart> itemcart, int oid);
        void DeleteOrder(Order od);
        void DeleteListItemOrder(List<ItemOrder> listitemorder);
    }
    public class OrderRepository : IOrder
    {
        private readonly ShopContext _order;

        public OrderRepository(ShopContext order)
        {
            _order = order;
        }

        public int AddOrder(int accid)
        {
            var order = new Order
            {
                AccountId = accid,
                OrderStatus = 0,
                OrderDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"),
                TotalAmount = 0
            };
            _order.Add(order);
            _order.SaveChanges();
            return order.OId;
        }

        public void UpOrder(Order od, int shipid, int payid)
        {
            od.ShipId = shipid;
            od.PaymentId = payid;
            _order.SaveChanges();
        }

        public void UpOrderStatus(Order od, int status)
        {
            od.OrderStatus = status;
            _order.SaveChanges();
        }

        public void UpTotal(List<ItemOrder> litod, Order od)
        {
            decimal total = 0;
            foreach( var indexer in litod)
            {
                var price = _order.Product
                                  .Where(i => i.ProductId == indexer.ProdductId)
                                  .Select(i => i.Price)
                                  .FirstOrDefault();
                int quantity = Convert.ToInt32(indexer.Quantity);
                total =+ price * quantity;
            }
            od.TotalAmount = total;
            _order.SaveChanges();
        }

        public void UpQuantity(List<ItemOrder> litod,Order od)
        {
              
                foreach (var indexer in litod)
                {                
                    var item = _order.ListQuantity
                                      .Where(i => i.ProductId == indexer.ProdductId && i.SizeId == indexer.SizeId)
                                      .FirstOrDefault();
                    int oldquantity = Convert.ToInt32(item.Quantity);
                    int quantity = Convert.ToInt32(indexer.Quantity);
                    if(od.OrderStatus == 1)
                    {
                        item.Quantity = oldquantity - quantity;
                        _order.SaveChanges();
                    }
                    if(od.OrderStatus == 4) 
                    {
                        item.Quantity = oldquantity + quantity;
                        _order.SaveChanges();
                    }    
                    
                }                    
        }

        public Order? CheckOId(int id)
        {
            var od = _order.Order
                           .SingleOrDefault(i => i.OId == id);
            return od;
        }

        public List<Order> GetListOrder(int id)
        {
            var listorder = _order.Order
                                  .Where(i => i.AccountId == id)
                                  .ToList();
            return listorder;
        }

        public bool AddItemOrder(int oid, ItemOrder ito)
        {
            var oldQuantity = _order.ListQuantity
                                    .FirstOrDefault (i => i.ProductId == ito.ProdductId && i.SizeId == ito.SizeId);
            if(oldQuantity != null && oldQuantity.Quantity < ito.Quantity) 
                return false;
            if(oldQuantity != null && oldQuantity.Quantity >= ito.Quantity)
            {
                ItemOrder it = new ItemOrder()
                {
                    OId = oid,
                    ProdductId = ito.ProdductId,
                    SizeId = ito.SizeId,
                    Quantity = ito.Quantity
                };
                _order.Add(it);
                _order.SaveChanges();
                return true;
            }
            return false;
            
        }
        public List<ItemOrder> GetListItemOrder(int oid)
        {
            var listitemorder = _order.ItemOrder
                                      .Where(i => i.OId == oid)
                                      .ToList();
            return listitemorder;

        }
        public void ItemCartToItemOrder(List<ItemCart> itemcart, int oid)
        {
            foreach (var item in itemcart)
            {
                var oldQuantity = _order.ListQuantity
                                    .FirstOrDefault(i => i.ProductId == item.ProductId && i.SizeId == item.SizeId);                
                if (oldQuantity != null && oldQuantity.Quantity >= item.Quantity)
                {
                    ItemOrder add = new ItemOrder()
                    {
                        OId = oid,
                        ProdductId = item.ProductId,
                        SizeId = item.SizeId,
                        Quantity = item.Quantity

                    };
                    _order.Add(add);
                    _order.ItemCart.Remove(item);
                    _order.SaveChanges();
                }                   
            }            
        }
        public void DeleteOrder(Order od)
        {
            if (od != null)
            {
                _order.Order.Remove(od);
                _order.SaveChanges();
            }

        }
        public void DeleteListItemOrder(List<ItemOrder> listitemorder)
        {
            if (listitemorder.Any())
            {
                _order.ItemOrder.RemoveRange(listitemorder);
                _order.SaveChanges(); 
            }
        }
        public bool CheckQuantityOrder(int oldQ, int newQ)
        {
            if(oldQ < newQ)
                return false;
            return true;
        }
    }
}
