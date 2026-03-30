using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
   public class ProductTranslations
    {
        public int Id { get; set; }
        public String Name { get; set; }
     
        public String Description { get; set; }
        public String Language { get; set; } = "en";
        public int ProductId { get; set; }
        public Product Product { get; set; }


    }
}
