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
    public class BrandService: IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFileService _fileService;
        public BrandService(IBrandRepository brandRepository, IFileService fileService)
        {
            _brandRepository = brandRepository;
            _fileService = fileService;
        }

        public async Task CreateBrandAsync(BrandRequest request)
        {
            var brand = request.Adapt<Brand>();
            if (request.Logo != null)
            {
                brand.Logo = await _fileService.UploadFileAsync(request.Logo);
            }
            await _brandRepository.CreateAsync(brand);

        }

        public async Task<bool> DeleteBrand(int id)
        {
          var brand =await  _brandRepository.GetOne(b => b.Id == id);
            if (brand == null) return false;
            _fileService.DeleteFile(brand.Logo);
          return await  _brandRepository.DeleteAsync(brand);
        }

        public async Task<List<BrandResponse>> GetAllBrands()
        {
            var brands = await _brandRepository.GetAllAsync(b=> b.State == EntityStatus.Active, new[]
            {
        nameof(Brand.CreatedBy),
        nameof(Brand.Translations)
             });

            return brands.Adapt<List<BrandResponse>>();
        }
        public async Task<BrandResponse?> GetBrand(Expression<Func<Brand, bool>> filter)
        {
            var brand = await _brandRepository.GetOne(filter, new[]
            {  nameof(Brand.CreatedBy),
        nameof(Brand.Translations)});
            if (brand == null)
            {
                return null;
            }

            return brand.Adapt<BrandResponse>();
        }


    }
}
