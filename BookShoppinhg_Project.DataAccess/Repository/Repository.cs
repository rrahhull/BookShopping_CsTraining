using BookShopping_Project.DataAccess.Data;
using BookShoppinhg_Project.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppinhg_Project.DataAccess.Repository
{
    public class Repository<t>: iRepository<t> where t : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<t> DbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.DbSet = _context.Set<t>();
        }

        public void Add(t entity)
        {
            DbSet.Add( entity); //save

        }

        public t FirstorDefault(Expression<Func<t, bool>> filter = null, string IncludeProperties = null) //for multiple tables
        {
            IQueryable<t> query = DbSet;
            if (filter != null)
                query = query.Where(filter);
            if (IncludeProperties != null)
            {
                foreach (var IncludeProp in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(IncludeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public t Get(int id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<t> GetAll(Expression<Func<t, bool>> filter = null, Func<IQueryable<t>, IOrderedQueryable<t>> orderby = null, string IncludeProperties = null)
        {
            IQueryable<t> query = DbSet;
            if (filter != null)
                query = query.Where(filter);
            if (IncludeProperties != null)
            {
                foreach (var IncludeProp in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(IncludeProp);
                }
            }
            if (orderby != null)
                return orderby(query).ToList();
            return query.ToList();
        }

        public void remove(int id)
        {
            t entity = DbSet.Find(id);
            remove(entity);
        }

        public void remove(t entity)
        {
            DbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<t> entity)
        {
            DbSet.RemoveRange(entity);
        }
    }

}
