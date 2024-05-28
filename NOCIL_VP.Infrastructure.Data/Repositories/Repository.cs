using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Infrastructure.Interfaces.Repositories;

namespace NOCIL_VP.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly VpContext _dbContext;
        private readonly DbSet<T> _dbSet;
        private bool _disposed = false;

        public Repository(VpContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        
        public virtual T Add(T entity)
        {
            var res = _dbSet.Add(entity);
            return res.Entity;
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            this._dbContext.ChangeTracker.Clear();
            this._dbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            this._dbSet.UpdateRange(entities);
        }

        public virtual void SoftDelete(T entity)
        {
            this._dbContext.ChangeTracker.Clear();
            this._dbSet.Update(entity);
        }

        public virtual void Remove(T entity)
        {
            this._dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            this._dbSet.RemoveRange(entities);
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }


        public bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _dbContext.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
