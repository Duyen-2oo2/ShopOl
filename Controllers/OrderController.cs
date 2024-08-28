using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models;
using ShopOnline.Service;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _order;
        private readonly IItemCart _itemcart;
        private readonly IListQuantity _listQuantity;
        public OrderController(IOrder order, IItemCart itemcart, IListQuantity listQuantity)
        {
            _order = order;
            _itemcart = itemcart;
            _listQuantity = listQuantity;
        }

        [HttpGet("GetListOrder")]
        public IActionResult GetListOrder(int accid)
        {
            var order = _order.GetListOrder(accid);
            return Ok(order);
        }

        [HttpGet("GetOrderDetail")]
        public IActionResult GetOrderDetail(int oid)
        {
            return Ok(_order.GetListItemOrder(oid));
        }

        [HttpPost("AddOrderAndItem")]
        public IActionResult AddOrderAndItem(int accid, ItemOrder item)
        {
            _order.AddItemOrder(_order.AddOrder(accid), item);
            var od = _order.CheckOId(_order.AddOrder(accid));
            var list = _order.GetListItemOrder(od.OId);
            _order.UpTotal(list, od);
            return Ok();
        }

        [HttpPut("UpdateShipPayment")]
        public IActionResult UpdateShipPayment(int odid, int shipid, int payid)
        {
            try
            {
                _order.UpOrder(_order.CheckOId(odid), shipid, payid);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateOrderStatus")]
        public IActionResult UpdateOrderStatus(int odid, int status)
        {
            try
            {
                var od = _order.CheckOId(odid);
                _order.UpOrderStatus(od, status);
                var list = _order.GetListItemOrder(od.OId);
                _order.UpQuantity(list, od);
                _listQuantity.UpProductNote();
                return Ok("Thanh cong");
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpPost("ItemCartToItemOrder")]
        public IActionResult ItemCartToItemOrder(int cartid, int accid)
        {
            var icart = _itemcart.GetListItemCart(cartid);
            var oid = _order.AddOrder(accid);
            _order.ItemCartToItemOrder(icart, oid);
            var od = _order.CheckOId(oid);
            if (od == null)
            {
                return NotFound();
            }
            var list = _order.GetListItemOrder(oid);
            _order.UpTotal(list, od);
            return NoContent();

        }
        [HttpDelete("ReturnProduct")]
        public IActionResult DeleteOrder(int oid)
        {
            var od = _order.CheckOId(oid);
            if (od.OrderStatus == 1 || od.OrderStatus == 2)
            {
                _order.UpOrderStatus(od, 4);
                var list = _order.GetListItemOrder(oid);
                _order.UpQuantity(list, od);
            }
            _order.DeleteListItemOrder(_order.GetListItemOrder(oid));
            _order.DeleteOrder(od);
            _listQuantity.UpProductNote();
            return NoContent();
        }

    }
}
