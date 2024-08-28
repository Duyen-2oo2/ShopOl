using ShopOnline.Models;

namespace ShopOnline.Service
{
    public interface IListQuantity
    {
        List<Size> getProductSize(int proId);
        ListQuantity? getCheck(int proid, int sizeid);
        int getProductQuantity(ListQuantity quantity);
        void UpdateProductQuantity(ListQuantity oldQuantity, int newQuantity);
        bool DeleteProductSize(ListQuantity delete);
        void AddProductQuantity(ListQuantity list);
        void UpProductNote();

    }

    public class ListQuantityRepository : IListQuantity
    {
        private readonly ShopContext _listQquantity;

        public ListQuantityRepository(ShopContext listquantity)
        {
            _listQquantity = listquantity;
        }

        public void AddProductQuantity(ListQuantity list)
        {
            var check = _listQquantity.ListQuantity
                                      .FirstOrDefault(x => x.ProductId == list.ProductId && x.SizeId == list.SizeId);

            if (check == null)
            {
                _listQquantity.Add(list);
            }
            else
            {
                check.Quantity += list.Quantity;
            }

            _listQquantity.SaveChanges();

        }

        public bool DeleteProductSize(ListQuantity delete)
        {
            if (delete != null)
            {
                _listQquantity.Remove(delete);
                _listQquantity.SaveChanges();
                return true;
            }
            return false;
        }

        public ListQuantity? getCheck(int proid, int sizeid)
        {
            var quantity = _listQquantity.ListQuantity
                                         .SingleOrDefault(s => s.SizeId == sizeid && s.ProductId == proid);
            return quantity;
        }

        public int getProductQuantity(ListQuantity quantity)
        {
            if (quantity == null)
            {
                return 0;
            }
            else
                return quantity.Quantity;

        }

        public List<Size> getProductSize(int proId)
        {
            var listSize = _listQquantity.ListQuantity
                           .Where(l => l.ProductId == proId)
                           .Select(l => l.SizeId)
                           .ToList();
            List<Size> list = new List<Size>();
            foreach (var item in listSize)
            {
                var size = _listQquantity.Size
                                         .Where(s => s.SizeId == item)
                                         .Select(s => s)
                                         .SingleOrDefault();
                if (size != null)
                    list.Add(size);
            }
            return list;
        }

        public void UpdateProductQuantity(ListQuantity oldQuantity, int newQuantity)
        {
            oldQuantity.Quantity = newQuantity;
            _listQquantity.SaveChanges();
        }

        public void UpProductNote()
        {
            var list = _listQquantity.ListQuantity.ToList();
                            
            foreach (var item in list)
            {
                var pro = _listQquantity.Product
                                        .FirstOrDefault(p => p.ProductId == item.ProductId);
                if (item.Quantity == 0)
                {
                    pro.Note = "Da het";
                    _listQquantity.SaveChanges();
                }
                if (item.Quantity > 0)
                {
                    pro.Note = "Dang ban";
                    _listQquantity.SaveChanges();
                }
            }
            

        }
    }
}

