using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService; 
        public ProductService(IProductRepository productRepository, IFileService fileService) { 
            _productRepository = productRepository;
            _fileService = fileService;
        }

        public async Task CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();

            if (request.MainImage != null)
            {
                //روح جبلي اسم الصوره و ارفعها عشان تجيبلي لينك الصوره
                var imagePath = await _fileService.UploadFileAsync(request.MainImage);
                product.MainImage = imagePath;

            }
            await _productRepository.CreateAsync(product);
        }

        public async Task<List<ProductResponse>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync(new[]
            {
                nameof(Product.CreatedBy),nameof(Product.Translations) 

            });
            var response = products.Adapt<List<ProductResponse>>();
            return response;

        }
        public async Task<ProductResponse?> GetProduct(Expression<Func<Product,bool>>filter)
        {
            var products = await _productRepository.GetOne(filter, new[]
            {
                nameof(Product.CreatedBy),nameof(Product.Translations)

            });
            if (products == null)
                return null;
            var response = products.Adapt<ProductResponse>();
            return response;

        }
        public async Task<bool> DeleteProduct(int id)
        {

            var product = await _productRepository.GetOne(c => c.Id == id);
         
            if (product == null)
                return false;
            _fileService.DeleteFile(product.MainImage);
            return await _productRepository.DeleteAsync(product);

        }














    }
}
