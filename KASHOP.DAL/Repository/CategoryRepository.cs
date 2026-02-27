using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    //ICategoryRepository  اذا في ميثود مميز للكاتيجوري ومش موجود بالجيناريك  
    public class CategoryRepository :GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}

public class  Person
{
 public  Person(int x)
    {

    }
}
public class User: Person
{
  public  User(int x) : base(x)
    {
    }
}