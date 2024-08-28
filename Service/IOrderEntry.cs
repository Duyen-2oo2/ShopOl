using ShopOnline.Models;

namespace ShopOnline.Service
{
    public interface IOrderEntry
    {
        int AddOrderEntry(int accid);
        void AddItemOrderEntry(int id, ItemOrderEntry ito);
        OrderEntry? CheckOId(int id);
        List<OrderEntry> GetListOrderEntry(int id);
        
        void UpOrderEntryStatus(OrderEntry od, int status);
        void UpTotal(OrderEntry od);
        void UpQuantity(OrderEntry od);
        List<ItemOrderEntry> GetListItemOrderEntry(int oid);
        void DeleteOrderEntry(OrderEntry od);
        void DeleteListItemOrderEntry(List<ItemOrderEntry> listitemorderEntry);
    }
    public class OrderEntryRepository : IOrderEntry
    {
        private readonly ShopContext _orderEntry;

        public OrderEntryRepository(ShopContext orderEntry)
        {
            _orderEntry = orderEntry;
        }

        public int AddOrderEntry(int accid)
        {
            var orderEntry = new OrderEntry
            {
                AccountId = accid,
                OEStatus = 0,
                DateOE = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                TotalAmount = 0
            };
            _orderEntry.Add(orderEntry);
            _orderEntry.SaveChanges();
            return orderEntry.OEId;
        }

        

        public void UpOrderEntryStatus(OrderEntry od, int status)
        {
            od.OEStatus = status;
            _orderEntry.SaveChanges();
        }

        public void UpTotal(OrderEntry od)
        {
            decimal total = 0;
            var it = _orderEntry.ItemOrderEntry
                           .Where(i => i.OEId == od.OEId)
                           .ToList();
            foreach (var indexer in it)
            {
                var price = _orderEntry.Product
                                  .Where(i => i.ProductId == indexer.ProductId)
                                  .Select(i => i.EntryPrice)
                                  .FirstOrDefault();
                int quantity = Convert.ToInt32(indexer.EQuantity);
                total = +price * quantity;
            }
            od.TotalAmount = total;
            _orderEntry.SaveChanges();
        }

        public void UpQuantity(OrderEntry od)
        {
            var it = _orderEntry.ItemOrderEntry
                         .Where(i => i.OEId == od.OEId)
                         .ToList();
            foreach (var indexer in it)
            {
                int newquantity = 0;
                var item = _orderEntry.ListQuantity
                                  .Where(i => i.ProductId == indexer.ProductId && i.SizeId == indexer.SizeId)
                                  .FirstOrDefault();
                int oldquantity = Convert.ToInt32(item.Quantity);
                int quantity = Convert.ToInt32(indexer.EQuantity);
                if (od.OEStatus == 1)
                {
                    newquantity = oldquantity - quantity;
                    item.Quantity = newquantity;
                    _orderEntry.SaveChanges();
                }              
            }
        }

        public OrderEntry? CheckOId(int id)
        {
            var od = _orderEntry.OrderEntry
                           .SingleOrDefault(i => i.OEId == id);
            return od;
        }

        public List<OrderEntry> GetListOrderEntry(int id)
        {
            var listorderEntry = _orderEntry.OrderEntry
                                  .Where(i => i.AccountId == id)
                                  .ToList();
            return listorderEntry;
        }

        public void AddItemOrderEntry(int oeid, ItemOrderEntry itoe)
        {
            ItemOrderEntry it = new ItemOrderEntry()
            {
                OEId = oeid,
                ProductId = itoe.ProductId,
                EQuantity = itoe.EQuantity
            };
            _orderEntry.Add(it);
            _orderEntry.SaveChanges();
        }
        public List<ItemOrderEntry> GetListItemOrderEntry(int oeid)
        {
            var listitemorderEntry = _orderEntry.ItemOrderEntry
                                      .Where(i => i.OEId == oeid)
                                      .ToList();
            return listitemorderEntry;

        }
        
        public void DeleteOrderEntry(OrderEntry od)
        {
            if (od != null)
            {
                _orderEntry.OrderEntry.Remove(od);
                _orderEntry.SaveChanges();
            }

        }
        public void DeleteListItemOrderEntry(List<ItemOrderEntry> listitemorderEntry)
        {
            if (listitemorderEntry.Any())
            {
                _orderEntry.ItemOrderEntry.RemoveRange(listitemorderEntry);
                _orderEntry.SaveChanges();
            }
        }
    }
}
