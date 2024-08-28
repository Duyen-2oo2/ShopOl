using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models;
using ShopOnline.Service;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipController : ControllerBase
    {
        private readonly ICPSS<Ship> _ship;
        public ShipController(ICPSS<Ship> ship)
        {
            _ship = ship;
        }

        [HttpGet("GetAllShip")]
        public IActionResult GetCategories()
        {
            var categories = _ship.getAll().ToList();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetShip(int id)
        {
            var ship = _ship.getById(id);
            if (ship == null)
                return NotFound();
            return Ok(ship);
        }

        [HttpPost]
        public IActionResult PostShip(Ship newShip)
        {
            var check = _ship.Add(newShip);
            if (check == 1)
                return Ok();
            return BadRequest();
        }

        [HttpPut("UpdateShip")]
        public IActionResult PutShip(int id, Ship upShip)
        {
            var o = _ship.getById(id);
            if (o == null)
            {
                return NotFound();
            }
            _ship.Update(o, upShip);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteShip(int id)
        {
            _ship.Delete(id);
            return NoContent();

        }
    }
}
