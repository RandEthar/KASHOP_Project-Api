using KASHOP.BLL.Service;
using KASHOP.DAL.Data;
using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using KASHOP.PL.Resource;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        // readonly يعني ما بقدر اعدل عليها بعد ما اعطيها قيمة في الكونستركتور
      
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService, IStringLocalizer<SharedResources> localizer)
        {
          
            _localizer = localizer; _categoryService = categoryService;
        }
        [HttpPost("")]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
        var response=await _categoryService.CreateCategory(request);
            return Ok(new
            {
                message = _localizer["success"].Value,data = response
            });

        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var Categories=await _categoryService.GetAllCategories();

            return Ok(new
        {

                message =_localizer["success"].Value,data= Categories,
              
            });
        
        }


    }
}
