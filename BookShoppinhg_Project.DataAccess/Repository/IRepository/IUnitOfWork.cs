using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppinhg_Project.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IShoppingCartReposiory shoppingCart { get; }
        IOrderHeaderRepository orderHeader { get; }
        IOrderDetailsRepository orderDetails { get; }
        IProductRepository Product { get; }
        ISP_CALL SP_CALL { get; }
        iCategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        IApplicationUserRepository applicationUser { get; }
        ICompanyRepository company { get; }
        void Save();
    }
}
