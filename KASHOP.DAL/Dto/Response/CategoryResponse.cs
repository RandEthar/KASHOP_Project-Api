using KASHOP.DAL.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Dto.Response
{
    public class CategoryResponse
    {
        public List<CategoryTranslationResponce> Translations { get; set; }

    }
}
