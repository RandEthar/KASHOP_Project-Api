using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Mapping
{
    public static class MapsterConfig
    {
        public static void MapesterConfigRegister()
        {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.Category_Id, src => src.Id)
                .Map(dest => dest.UserCreated, src => src.CreatedBy.UserName)
                .Map(dest => dest.Name, src => src.Translations
                .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                .Select(n => n.Name).FirstOrDefault());


                   TypeAdapterConfig<Product, ProductResponse>.NewConfig()
   
                .Map(dest => dest.MainImage, src => $"https://localhost:7095/Images/{src.MainImage}")
                .Map(dest => dest.UserCreated, src => src.CreatedBy.UserName)
                .Map(dest => dest.Name, src => src.Translations
               .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
               .Select(n => n.Name).FirstOrDefault());



            TypeAdapterConfig<Brand, BrandResponse>.NewConfig()

         .Map(dest => dest.Logo, src => $"https://localhost:7095/Images/{src.Logo}")
         .Map(dest => dest.UserCreated, src => src.CreatedBy.UserName)
         .Map(dest => dest.Name, src => src.Translations
        .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
        .Select(n => n.Name).FirstOrDefault());



        }





    
    } }
 