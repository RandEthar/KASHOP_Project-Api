using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{  // بياخد كل الاشياء يلي جوا كلاس ال IdentityUser زي ال username وال password و email و غيرها
    //وبضيف عليهم يلي بده اياه
    public class ApplicationUser: IdentityUser
    {
      public String FullName { get; set; }
         public String? City { get; set; }
         public String? Street { get; set; }  
        //عشان نقارن الكود يلي بعتناه بالكود يلي كتبه اليوزر منخزنه بجدول اليوزر
            public String? CodeResetPassword{ get; set; }
        //عشان نحدد وقت انتهاء صلاحية الكود يلي بعتناه لليوزر
        public DateTime? CodeResetPasswordExpiration { get; set; }
    }
}
