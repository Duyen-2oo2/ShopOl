using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models;
using ShopOnline.Service;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ICPSS<Payment> _payment;
        public PaymentController(ICPSS<Payment> payment)
        {
            _payment = payment;
        }

        [HttpGet("GetAllPayment")]
        public IActionResult GetCategories()
        {
            var categories = _payment.getAll().ToList();
            
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetPayment(int id)
        {
            var payment = _payment.getById(id);

            return Ok(payment);
        }

        [HttpPost]
        public IActionResult PostPayment(Payment newPayment)
        {
            _payment.Add(newPayment);
            return CreatedAtAction(nameof(GetPayment), new { id = newPayment.PaymentId }, newPayment);

        }

        [HttpPut("UpdatePayment")]
        public IActionResult PutPayment(int id, Payment upPayment)
        {
            var o = _payment.getById(id);
            if (o == null)
            {
                return NotFound();
            }
            _payment.Update(o, upPayment);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeletePayment(int id)
        {
            _payment.Delete(id);
            return NoContent();

        }
    }
}
