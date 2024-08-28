using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models;
using ShopOnline.Service;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _account;
        public AccountController(IAccount account)
        {
            _account = account;
           
        }

        [HttpPost("signUp")]
        public ActionResult SignUp(string user, string pass)
        {
            if (_account.checkUser(user) == null)
            {             
                _account.AddCart(_account.signUp(user, pass));
                return Ok();
            }
            return NotFound("Ten da ton tai");
        }

        [HttpGet("login")]
        public ActionResult Login(string user, string pass)
        {
            var account = _account.logIn(user, pass);
            if (account == false)
            {
                return NotFound("Tên người dùng hoặc mật khẩu không đúng.");
            }

            return Ok(account);
        }
        

        [HttpGet("GetAccountId")]
        public IActionResult GetAccountId(int id)
        {
            var acc = _account.getAccountInfo(id);      
            if (acc == null)
                return NotFound("tkktt");
            return Ok(acc);
        }

        [HttpPut("UpAccountInfo")]
        public IActionResult PutInfo(Account upInfo, int id)
        {
            var acc = _account.getAccountInfo(id);
            if (acc == null)
                return NotFound();
            _account.updateInfo(acc, upInfo);
            return NoContent();
        }

        [HttpPut("UpAccountPass")]
        public IActionResult PutPass(string upPass, string user)
        {
           var acc =  _account.checkUser(user);
            if (acc == null)
                return NotFound("Tai khoan khong ton tai");
            _account.forGetPass(acc, upPass);
            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteAccount(int id)
        {
            var acc = _account.getAccountInfo(id);
            if (acc == null)
                return NotFound("Tai khoan khong ton tai");
            _account.Delete(acc);
            return NoContent();

        }
    }
}
