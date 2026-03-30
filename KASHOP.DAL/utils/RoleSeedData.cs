using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.utils
{
   public class RoleSeedData : ISeedData
    {

        //فش داعي اضيفه في program لانه identity framework بيضيفه اوتوماتيك
        RoleManager<IdentityRole> _roleManager;
        public RoleSeedData(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        //اضافه داتا على جدول الرولز
        public async Task DataSeed()
        {
           String [] roles = { "Admin", "User" ,"SuperAdmin"};
            if (!await _roleManager.Roles.AnyAsync())//تحقق إذا كان جدول الـ Roles فاضي في قاعدة البيانات
             {
                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));//لو فاضي اضف الرولز اللي في المصفوفه
                }
             }
           

        }
    }
}
