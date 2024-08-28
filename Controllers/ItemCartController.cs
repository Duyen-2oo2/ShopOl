using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models;
using ShopOnline.Service;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCartController : ControllerBase
    {
        private readonly IItemCart _itemcart;
        public ItemCartController(IItemCart itemcart)
        {
            _itemcart = itemcart;
        }
        [HttpGet("GetListItemCartId")]
        public IActionResult GetListItemCartId(int id)
        {
            try
            {
                var listitem = _itemcart.GetListItemCart(id);
                return Ok(listitem);
            }
            catch(Exception ex) 
            { 
                return NotFound(ex); 
            }
            
        }
        [HttpPost("AddItemCart")]
        public IActionResult AddItemCart(int id, int proid, int sizeid, int quantity) 
        {
            _itemcart.AddItemCart(id, proid,sizeid,quantity);            
            return Ok();
        }

        [HttpPut("UpdateItemCart")]
        public IActionResult UpdateItemCart(int id, ItemCart itc)
        { 
                var itco = _itemcart.CheckIcId(id);
            if (itco != null)
            {
                _itemcart.UpdateItemCart(itco, itc);
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete("DeleteItemCart")]
        public IActionResult DeleteItemCart(int id)
        {
            var itco = _itemcart.CheckIcId(id);
            if (itco != null)
            {
                _itemcart.RemoveItemCart(itco);
                return Ok();
            }
            return NotFound();
        }   
    }
}
