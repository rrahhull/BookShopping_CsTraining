using BookShopping_Project.DataAccess.Data;
using BookShoppinhg_Project.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppinhg_Project.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            CoverType = new CoverTypeRepository(_context);
            SP_CALL = new SP_CALL(_context);
            Product = new ProductRepository(_context);
            applicationUser = new ApplicationUserRepository(_context);
            company = new CompanyRepository(_context);
            shoppingCart = new ShoppingCartRepository(_context);
            orderDetails = new OrderDetailsRepository(_context);
            orderHeader = new OrderHeaderRepository(_context);

        }
        public ICompanyRepository company { get; private set; }
        public IProductRepository Product { get; private set; }
        public ISP_CALL SP_CALL { get; private set; }
        public iCategoryRepository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public IApplicationUserRepository applicationUser { get; private set; }

        public IShoppingCartReposiory shoppingCart { get; private set; }

        public IOrderHeaderRepository orderHeader { get; private set; }

        public IOrderDetailsRepository orderDetails { get; private set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
