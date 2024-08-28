using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemOrderEntryController : ControllerBase
    {
        private readonly ShopContext _context;
        public ItemOrderEntryController(ShopContext context)
        {
            _context = context;
        }



        [HttpGet("OEId")]
        public IActionResult GetItemOrderEntry(int oid)
        {
            if (_context.ItemOrderEntry == null)
                return NotFound();
            var itoe = _context.ItemOrderEntry.Find(oid);
            if (itoe == null)
                return NotFound();
            return Ok(itoe);
        }

        [HttpPost]
        public IActionResult PostItemOrderEntry(ItemOrderEntry newItemOrderEntry)
        {
            _context.ItemOrderEntry.Add(newItemOrderEntry);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetItemOrderEntry), new { id = newItemOrderEntry.IOEId }, newItemOrderEntry);

        }

        [HttpPut]
        public IActionResult PutItemOrderEntry(int id, ItemOrderEntry upItemOrderEntry)
        {
            var itemOE = _context.ItemOrderEntry.Find(id);
            if (itemOE == null)
                return BadRequest();
            itemOE.EQuantity = upItemOrderEntry.EQuantity;
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteItemOrderEntry(int oid, int proid)
        {
            if (_context.ItemOrderEntry == null)
                return NotFound();
            var oe = _context.OrderEntry.Find(oid);
            if (oe == null)
                return NotFound();
            var itemOE = _context.ItemOrderEntry
                                    .Where(itoe => itoe.OEId == oid && itoe.ProductId == proid)
                                    .SingleOrDefault();

            if (itemOE == null)
                return NotFound();

            _context.ItemOrderEntry.Remove(itemOE);
            _context.SaveChanges();
            return NoContent();

        }
    }
}
