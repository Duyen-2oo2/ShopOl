using ShopOnline.Models;

namespace ShopOnline.Service
{
    public interface ICPSS<T> where T : class
    {
        List<T> getAll();
        
        T? getById(int id);

        int Add(T t);
        void Update(T o, T n);

        void Delete(int id);

    }
}
