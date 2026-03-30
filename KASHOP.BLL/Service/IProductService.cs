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
    public interface IProductService
    {
      Task CreateProduct(ProductRequest request);
        Task<List<ProductResponse>> GetAllProducts();
        Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter);
        Task<bool> DeleteProduct(int id);
    }
}
