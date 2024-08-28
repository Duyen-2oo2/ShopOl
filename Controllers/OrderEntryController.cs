using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models;
using ShopOnline.Service;
using System.Text;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderEntryController : ControllerBase
    {
        private readonly IOrderEntry _orderEntry;
        private readonly IListQuantity _listQuantity;
        public OrderEntryController(IOrderEntry orderEntry, IListQuantity listQuantity)
        {
            _orderEntry = orderEntry;
            _listQuantity = listQuantity;
        }

        [HttpGet("GetListOrderEntry")]
        public IActionResult GetListOrderEntry(int accid)
        {
            var orderentry = _orderEntry.GetListOrderEntry(accid);
            return Ok(orderentry);
        }

        [HttpGet("GetOrderEntryDetail")]
        public IActionResult GetOrderEntryDetail(int oeid)
        {
            var check = _orderEntry.GetListItemOrderEntry(oeid);
            if (check.Count != 0) 
                return Ok(check);
            return NotFound("Khong co OrderEntry");
        }

        [HttpPost("AddOrderEntry")]
        public IActionResult AddOrderEntry(int accid)
        {
            _orderEntry.AddOrderEntry(accid);
            return Ok();
        }
        [HttpPost("AddItemOrderEntry")]
        public IActionResult AddItemOrderEntry(int oeid, ItemOrderEntry itoe)
        {
            _orderEntry.AddItemOrderEntry(oeid, itoe);
            var ode = _orderEntry.CheckOId(oeid);
            _orderEntry.UpQuantity(ode);
            _orderEntry.UpTotal(ode);
            _listQuantity.UpProductNote();

            return Ok();
        }


        [HttpDelete("DeleteOrderEntry")]
        public IActionResult DeleteOrderEntry(int oeid)
        {
            var od = _orderEntry.CheckOId(oeid);
            if (od != null)
            {
                if (od.OEStatus == 0)
                {
                    _orderEntry.DeleteListItemOrderEntry(_orderEntry.GetListItemOrderEntry(oeid));
                    _orderEntry.DeleteOrderEntry(od);
                    _listQuantity.UpProductNote();
                    return Ok();
                }
            }
            
            return NotFound("Khoong thay OrderEntry");
        }
    }
}
