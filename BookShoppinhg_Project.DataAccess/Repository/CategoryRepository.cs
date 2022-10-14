using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.Models;
using BookShoppinhg_Project.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppinhg_Project.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, iCategoryRepository
    {
        public readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public void update(Category category)
        {
            _context.Update(category);
        }
    }
}
