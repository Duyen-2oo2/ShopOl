using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models;
using ShopOnline.Service;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ICPSS<Supplier> _supplier;
        public SupplierController(ICPSS<Supplier> supplier)
        {
            _supplier = supplier;
        }

        [HttpGet("GetAllSupplier")]
        public IActionResult GetCategories()
        {
            var categories = _supplier.getAll().ToList();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetSupplier(int id)
        {
            var supplier = _supplier.getById(id);
            if (supplier == null)
                return NotFound();
            return Ok(supplier);
        }

        [HttpPost]
        public IActionResult PostSupplier(Supplier newSupplier)
        {
            var check = _supplier.Add(newSupplier);
            if (check == 1)
                return Ok();
            return BadRequest();

        }

        [HttpPut("UpdateSupplier")]
        public IActionResult PutSupplier(int id, Supplier upSupplier)
        {
            var o = _supplier.getById(id);
            if (o == null)
            {
                return NotFound();
            }
            _supplier.Update(o, upSupplier);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteSupplier(int id)
        {
            _supplier.Delete(id);
            return NoContent();

        }
    }
}
