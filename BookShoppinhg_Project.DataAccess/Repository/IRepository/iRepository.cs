using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppinhg_Project.DataAccess.Repository.IRepository
{
    public interface iRepository<t> where t : class
    {
        t Get(int id);
        IEnumerable<t> GetAll(Expression<Func<t, bool>> filter = null,Func<IQueryable<t>, IOrderedQueryable<t>> orderby = null,string IncludeProperties = null);
        t FirstorDefault(
            Expression<Func<t, bool>> filter = null,
            string IncludeProperties = null
            );
        void Add(t entity);
        void remove(int id);
        void remove(t entity);
        void RemoveRange(IEnumerable<t> entity);

    }
}
