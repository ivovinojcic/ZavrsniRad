using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VeterinarskaStanica.Model.Core;

namespace VeterinarskaStanica.Model.DatabaseConnector
{
   public abstract class RepositoryBase<T> where T : class
    {
        #region Properties
        private DataBaseConnection _dataContext;

        private readonly DbSet<T> _dbSet;

        private IDbFactory DbFactory
        {
            get;
        }

        protected DataBaseConnection DbContext => _dataContext ?? (_dataContext = DbFactory.Init());
        
        #endregion

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            _dbSet = DbContext.Set<T>();
        }

        #region Implementation
        
        public virtual async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = _dbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
                _dbSet.Remove(obj);
        }

        public virtual async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where)
        {
            return await _dbSet.Where(where).ToListAsync();
        }

        public async Task<T> Get(Expression<Func<T, bool>> where)
        {
            return await _dbSet.Where(where).FirstOrDefaultAsync();
        }

        public async Task<bool> Any(Expression<Func<T, bool>> where)
        {
            return await _dbSet.AnyAsync(where);
        }

        #endregion

    }
}
