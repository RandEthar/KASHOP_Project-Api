using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllCategories();
         Task<CategoryResponse> CreateCategory(CategoryRequest categoryResponse);
        Task<CategoryResponse> GetCategory(Expression<Func<Category,bool>> filter);
        Task<bool> DeleteCategory(int id);

    }
}
