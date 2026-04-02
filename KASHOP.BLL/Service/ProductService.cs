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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;
        public ProductService(IProductRepository productRepository, IFileService fileService)
        {
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
            var products = await _productRepository.GetAllAsync(
                p=>p.State == EntityStatus.Active
                , new[]
            {
                nameof(Product.CreatedBy),nameof(Product.Translations)

            });
            var response = products.Adapt<List<ProductResponse>>();
            return response;

        }
        public async Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter)
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

        public async Task<bool> UpdateProduct(int id, ProductUpdateRequest productUpdateRequest)
        {
            var productDb = await _productRepository.GetOne(c => c.Id == id,
                new string[] { nameof(Product.Translations)
            });
            if (productDb == null) return false;

            productUpdateRequest.Adapt(productDb);
            if (productUpdateRequest.Translations != null)
            {
                foreach (var translation in productUpdateRequest.Translations)
                {
                    var existing = productDb.Translations.FirstOrDefault(t => t.Language == translation.Language);
                    if (existing != null)
                    {
                        if (translation.Name != null) existing.Name = translation.Name;
                        if (translation.Description != null) existing.Description = translation.Description;
                    }

                    else
                    {
                        return false;
                    }
                }
            }
            var oldImagePath = productDb.MainImage;
            if (productUpdateRequest.MainImage != null)
            {
                _fileService.DeleteFile(oldImagePath);
                var newImagePath = await _fileService.UploadFileAsync(productUpdateRequest.MainImage);
                productDb.MainImage = newImagePath;
            }
            else
            {
                productDb.MainImage = oldImagePath;
            }

            return await _productRepository.UpdateAsync(productDb);

        }



        public async Task<bool> ToggleStatus(int id)
        {
            var productDb = await _productRepository.GetOne(c => c.Id == id);
            if (productDb == null) return false;
            productDb.State = productDb.State == EntityStatus.Active ? EntityStatus.Inactive : EntityStatus.Active;
            return await _productRepository.UpdateAsync(productDb);
        }
    }
}
