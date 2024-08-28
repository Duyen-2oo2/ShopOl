using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models;
using ShopOnline.Service;
using System.Text.Json;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _product;
        public ProductController(IProduct product)
        {
            _product = product;
        }

        [HttpGet("GetListProducts")]
        public IActionResult GetListProducts(int pageNumber, int pageSize = 2)
        {
            var products = _product.getList(pageNumber, pageSize);
            var totalProduct = _product.getProductAll().Count;
            var totalPage = (int)Math.Ceiling(totalProduct / (double)pageSize);

            var metadata = new
            {
                 totalProduct,
                 pageSize,
                 pageNumber,
                 totalPage,
                 HasPrevious = pageNumber > 1,
                 HasNext = pageNumber < totalPage
            };

            var metadataJson = JsonSerializer.Serialize(metadata);
            Response.Headers.Add("X-Pagination", metadataJson);
            return Ok(products);
        }

        [HttpGet("getProductById")]
        public IActionResult GetProductById(int id)
        {
            var product = _product.getProductById(id);
            return Ok(product);
        }

        [HttpPost]
        public IActionResult PostProduct(Product newProduct)
        {
            _product.AddProduct(newProduct);
            
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.ProductId }, newProduct);

        }
        [HttpPut("PutProduct")]
        public IActionResult PutProduct(Product proNew, int id)
        {
            var proOld = _product.getProductById(id);
            if (proOld == null)
                return NotFound();
            _product.Update(proOld, proNew);
            return NoContent();  
        }

        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _product.getProductById(id);
            if (product == null)
                return NotFound();
            _product.Delete(product);
            return NoContent();
        }
        
    }
}
