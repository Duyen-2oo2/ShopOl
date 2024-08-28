using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models;
using ShopOnline.Service;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ICPSS<Size> _size;
        public SizeController(ICPSS<Size> size)
        {
            _size = size;
        }

        [HttpGet("GetAllSize")]
        public IActionResult GetCategories()
        {
            var categories = _size.getAll().ToList();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetSize(int id)
        {
            var size = _size.getById(id);
            if (size == null)
                return NotFound();
            return Ok(size);
        }

        [HttpPost]
        public IActionResult PostSize(Size newSize)
        {
           
           var check = _size.Add(newSize);
            if(check ==1)
                return Ok();
            return BadRequest();
        }

        [HttpPut("UpdateSize")]
        public IActionResult PutSize(int id, Size upSize)
        {
            var o = _size.getById(id);
            if (o == null)
            {
                return NotFound();
            }
            _size.Update(o, upSize);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteSize(int id)
        {
            _size.Delete(id);
            return NoContent();

        }
    }
}
