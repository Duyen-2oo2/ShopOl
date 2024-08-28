using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models;
using ShopOnline.Service;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListQuantityController : ControllerBase
    {
        private readonly IListQuantity _lQuantity;
        private readonly ICPSS<Size> _size;
        public ListQuantityController(IListQuantity lQuantity, ICPSS<Size> size)
        {
            _lQuantity = lQuantity;
            _size = size;
        }
        [HttpGet("getProductSize")]
        public IActionResult getProductSize(int proId)
        {
            try
            {
                var lquantity = _lQuantity.getProductSize(proId);
                return Ok(lquantity);
            }
            catch
            {
                return NotFound("San pham chua cap nhat size");
            }
        }

        [HttpGet("getProductQuantity")]
        public IActionResult getProductQuantity(int proId, int sizeId)
        {
            var lquantity = _lQuantity.getCheck(proId,sizeId);
            var quantity = _lQuantity.getProductQuantity(lquantity);
            return Ok(quantity);
        }

        [HttpPut("UpdateProductQuantity")]
        public IActionResult PutProductQuantity(int proId, int sizeId, int newQuantity)
        {
            var lquantity = _lQuantity.getCheck(proId, sizeId);
             _lQuantity.UpdateProductQuantity(lquantity, newQuantity);
            return NoContent();
        }
        [HttpPost("AddProductQuantity")]
        public IActionResult AddProductQuantity(ListQuantity list)
        {
            _lQuantity.AddProductQuantity(list);
            return Ok();
        }

        [HttpDelete("DeleteProductSize")]
        public IActionResult DeleteProductSize(int proId, int sizeId)
        {
            var lquantity = _lQuantity.getCheck(proId, sizeId);
           var check =  _lQuantity.DeleteProductSize(lquantity);
            return Ok(check);
        }

    }
}
