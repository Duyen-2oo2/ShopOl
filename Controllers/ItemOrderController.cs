using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemOrderController : ControllerBase
    {
        private readonly ShopContext _context;
        public ItemOrderController(ShopContext context)
        {
            _context = context;
        }

        

        [HttpGet("OEId")]
        public IActionResult GetItemOrder(int oid)
        {
            if (_context.ItemOrder == null)
                return NotFound();
            var itemOrder = _context.ItemOrder.Find(oid);
            if (itemOrder == null)
                return NotFound();
            return Ok(itemOrder);
        }

        [HttpPost]
        public IActionResult PostItemOrder(ItemOrder newItemOrder)
        {
            _context.ItemOrder.Add(newItemOrder);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetItemOrder), new { id = newItemOrder.IOId }, newItemOrder);

        }

        [HttpPut]
        public IActionResult PutItemOrder(int id, ItemOrder upItemOrder)
        {
            var itemOrder = _context.ItemOrder.Find(id);
            if (itemOrder == null)
                return BadRequest();
            itemOrder.Quantity = upItemOrder.Quantity;
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteItemOrder(int oid,int proid)
        {
            if (_context.ItemOrder == null)
                return NotFound();
            var order = _context.Order.Find(oid);
            if (order == null)
                return NotFound();
            var itemOrder = _context.ItemOrder
                                    .Where(it => it.OId == oid && it.ProdductId == proid)
                                    .SingleOrDefault();

            if (itemOrder == null)
                return NotFound();
            
            _context.ItemOrder.Remove(itemOrder);
            _context.SaveChanges();
            return NoContent();

        }
    }
}
