using KASHOP.BLL.Service;
using KASHOP.DAL.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : Controller
    {
        private readonly IBrandService _brandService;
     public   BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var brands =await _brandService.GetAllBrands();
            return Ok(new
            {
                data= brands
            })
            ;
        }
        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> CreateBrand([FromForm] BrandRequest request)
        {
          await  _brandService.CreateBrandAsync(request);
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id )
        {
            var brand = await _brandService.GetBrand(b => b.Id == id);
            return Ok(new
            {
                data = brand
            })
            ;
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var isDeleted= await _brandService.DeleteBrand(id);
            if (!isDeleted)
            {
                return BadRequest();
            }
            return Ok();
        }





    }
}
