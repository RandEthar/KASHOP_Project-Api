using KASHOP.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public class BrandRepository:GenericRepository<Models.Brand>, IBrandRepository  
    {
            public BrandRepository(ApplicationDbContext context) : base(context)
            {
        }
    }
}
