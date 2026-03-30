using KASHOP.BLL.Service;
using KASHOP.DAL.Data;
using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using KASHOP.PL.Resource;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KASHOP.PL.Controllers
{
  
    [ApiController]
    [Route("api/[controller]")]
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
        [Authorize]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            var response = await _categoryService.CreateCategory(request);
            return Ok(new
            {
                message = _localizer["success"].Value,
                data = response
            });

        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
        
            var Categories = await _categoryService.GetAllCategories();

            return Ok(new
            {

                message = _localizer["success"].Value,
                data = Categories,

            });

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var Category = await _categoryService.GetCategory(c => c.Id == id);

            return Ok(new
            {

                message = _localizer["success"].Value,
                data = Category,

            });

        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteCategory(id);
            if (!deleted)
                return Ok(new
                {
                    message = _localizer["NotFound"].Value,
                });
               return Ok(new
              {
                message = _localizer["Success"].Value,
               });

        }

    }
}
