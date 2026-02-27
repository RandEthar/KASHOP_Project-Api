using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    public class CategoryTranslation
    {
        public int Id { get; set; }
        //  Foreign Key
        public int CategoryId { get; set; }
        public string Language { get; set; } = "en";
        public string Name { get; set; } = null!;
   
        public  Category Category { get; set; }

    }
}
