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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public void update(Product product)
        {
            var ProductInDb = _context.products.FirstOrDefault(p => p.id == product.id);
                if (ProductInDb != null)
            {
                if (product.ImageUrl != "")
                    ProductInDb.ImageUrl = product.ImageUrl;
                ProductInDb.Title = product.Title;
                ProductInDb.description = product.description;
                ProductInDb.ISBN = product.ISBN;
                ProductInDb.Author = product.Author;
                ProductInDb.ListPrice = product.ListPrice;
                ProductInDb.price50 = product.price50;
                ProductInDb.price100 = product.price100;
                ProductInDb.price = product.price;
                ProductInDb.CategoryId = product.CategoryId;
                ProductInDb.CoverTypeId = product.CoverTypeId;
            }
        }
    }
}
