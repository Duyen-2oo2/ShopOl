
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;

namespace ShopOnline.Service
{
    public class CPSSRepository<T> : ICPSS<T> where T : class
    {
        protected readonly ShopContext _context;

        public CPSSRepository(ShopContext context)
        {
            _context = context;
        }
        public int Add(T t)
        {
           
            if (t is Payment pay)
            {
                var check = _context.Payment.SingleOrDefault(p => p.PaymentName == pay.PaymentName);
                if(check == null)
                {
                    _context.Set<T>().Add(t);
                    _context.SaveChanges();
                    return 1;
                }
                return 0;
            }
            if(t is Category category)
            {
                var check = _context.Category.SingleOrDefault(p => p.CategoryName == category.CategoryName);
                if (check == null)
                {
                    _context.Set<T>().Add(t);
                    _context.SaveChanges();
                    return 1;
                }
                return 0;
            }
            if(t is Size size)
            {
                var check = _context.Size.SingleOrDefault(p => p.SizeName == size.SizeName);
                if (check == null)
                {
                    _context.Set<T>().Add(t);
                    _context.SaveChanges();
                    return 1;
                }
                return 0;
            }
            if(t is Supplier sup)
            {
                var check = _context.Supplier.SingleOrDefault(p => p.SupName == sup.SupName);
                if (check == null)
                {
                    _context.Set<T>().Add(t);
                    _context.SaveChanges();
                    return 1;
                }
                return 0;
            }
            if(t is Ship ship)
            {
                var check = _context.Ship.SingleOrDefault(p => p.ShipName == ship.ShipName);
                if (check == null)
                {
                    _context.Set<T>().Add(t);
                    _context.SaveChanges();
                    return 1;
                }
                return 0;
            }

            return -1;
        }

        public void Delete(int id)
        {
            var t = _context.Set<T>().Find(id);
            if (t == null)
            {
                throw new ArgumentException("Product not found", nameof(id));
            }
            _context.Set<T>().Remove(t);
            _context.SaveChanges();
        }

        public List<T> getAll()
        {
            var t = _context.Set<T>().ToList();           
            return t;
        }

        public T? getById(int id)
        {
            var t = _context.Set<T>().Find(id);           
            return t;
        }

        public void Update(T o, T n)
        {
            if (o is Payment oldPayment && n is Payment newPayment)
            {
                oldPayment.PaymentName = newPayment.PaymentName;
                _context.SaveChanges();
            }
            if (o is Category oldCategory && n is Category newCategory)
            {
                oldCategory.CategoryName = newCategory.CategoryName;
                _context.SaveChanges();
            }
            if (o is Supplier oldSupplier && n is Supplier newSupplier)
            {
                oldSupplier.SupName = newSupplier.SupName;
                _context.SaveChanges();
            }
            if (o is Size oldSize && n is Size newSize)
            {
                oldSize.SizeName = newSize.SizeName;
                _context.SaveChanges();
            }
            if (o is Ship old && n is Ship nem)
            {
                old.ShipName = nem.ShipName;
                old.ShipPrice = nem.ShipPrice;
                _context.SaveChanges();
            }

        }
         
    }
}
