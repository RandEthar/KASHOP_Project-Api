using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
  public  class CategoryService: ICategoryService
    {

      private readonly  ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse> CreateCategory(CategoryRequest request)
        {
            var category = request.Adapt<DAL.Models.Category>();
         await   _categoryRepository.CreateAsync(category);
            return category.Adapt<CategoryResponse>();

        }

        public async Task<List<CategoryResponse>> GetAllCategories()
        {
          var categories = await _categoryRepository.GetAllAsync();

            return categories.Adapt<List<CategoryResponse>>();
        }
    }
}
