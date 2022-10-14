using BookShopping_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppinhg_Project.DataAccess.Repository.IRepository
{
   public interface iCategoryRepository:iRepository<Category>
    {
        void update(Category category);
    }
}
