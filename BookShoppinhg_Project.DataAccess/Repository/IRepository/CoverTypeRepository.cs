using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppinhg_Project.DataAccess.Repository.IRepository
{
    class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public CoverTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void update(CoverType coverType)
        {
            _context.Update(coverType);
        }
    }
}
