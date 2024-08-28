using Azure;
using ShopOnline.Models;
using static ShopOnline.Service.ICart;

namespace ShopOnline.Service
{
    public interface IAccount
    {
        Account? checkUser(string user);
        int signUp(string user, string pass);

        bool logIn(string user, string pass);

        void forGetPass(Account acc, string pass);

        Account? getAccountInfo(int id);

        void updateInfo(Account infoOld, Account infoNew);       
        void Delete(Account product);
        void AddCart(int accId);
    }

    public class AccountRepository : IAccount
    {
        protected readonly ShopContext _context;

        public AccountRepository(ShopContext context)
        {
            _context = context;
        }
        public Account? checkUser(string user)
        {
            var acc = _context.Account.SingleOrDefault(a => a.UserName == user);
                return acc;          
        }
        public void Delete(Account account)
        {
            var cart = _context.Cart.SingleOrDefault(i => i.AccountId == account.AccountId);
            if(cart != null)
            {
                _context.Cart.Remove(cart);
                _context.SaveChanges();
            }
           
            _context.Account.Remove(account);
            _context.SaveChanges();
        }

        public void forGetPass(Account acc, string pass)
        {
           
                acc.Password = pass;
                _context.SaveChanges();
                
            
        }

        public Account? getAccountInfo(int id)
        {
           var acc = _context.Account.Find(id);
            return acc;
        }

        public bool logIn(string user, string pass)
        {
            
            var checkPass = BCrypt.Net.BCrypt.HashPassword(pass);
            var account = _context.Account
                .FirstOrDefault(a => a.UserName == user && a.Password == pass);
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass) || account == null)
            {
                return false;
            }
            return true;
        }

        public int signUp(string user, string pass)
        {           
            // Hash mật khẩu
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(pass);
            var account = new Account
            {
                UserName = user,
                Password = passwordHash
            };
            // Thêm và lưu account vào database
            _context.Account.Add(account);
            _context.SaveChanges();
            return account.AccountId;
        }

        public void updateInfo(Account infoOld, Account infoNew)
        {
            infoOld.FullName = infoNew.FullName;
            infoOld.PhoneNumber = infoNew.PhoneNumber;
            infoOld.Address = infoNew.Address;
            infoOld.Email = infoNew.Email;
            infoOld.Sex = infoNew.Sex;
            
            _context.SaveChanges();
        }
        public void AddCart(int accId)
        {
            var cart = new Cart()
            {
                AccountId = accId
            };
            _context.Add(cart);
            _context.SaveChanges();
        }
       
    }
}
