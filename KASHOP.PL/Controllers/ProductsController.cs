using Azure.Core;
using KASHOP.BLL.Service;
using KASHOP.DAL.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
          var products=  await _productService.GetAllProducts(); 
            return Ok(new
            {
                data=products
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByiD(int id)
        {
            var products = await _productService.GetProduct(p=>p.Id==id);
            return Ok(new
            {
                data = products
            });
        }

        [HttpPost("")]
        [Authorize]
        //[FromForm] لانه في اتريبيوت نوعه IFormFile
        public async Task<IActionResult> Create([FromForm]ProductRequest request)
        {
            await _productService.CreateProduct(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
   
        public async Task<IActionResult> Delete(int id)
        {
         var deleted=   await _productService.DeleteProduct(id);
            if (!deleted)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPatch("{id}")]
        [Authorize]

        public async Task<IActionResult> Update(int id, [FromForm] ProductUpdateRequest request)
        {
            var updated= await _productService.UpdateProduct(id, request);
            if (!updated)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPatch("status/{id}")]
        [Authorize]
        public async Task<IActionResult> toggleStatus(int id)
        {
            var updated = await _productService.ToggleStatus(id);
            if (!updated)
            {
                return BadRequest();
            }
            return Ok();
        }



    }
}
