using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Dto.Response
{
    public  class ProductResponse
    {
        public string UserCreated { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
   
        public string MainImage { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

    }
}
