using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _Repositories;

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _Repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
          return  await _dbContext.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_Repositories.ContainsKey(type))
            {
                var Repositoroy = new GenericRepository<TEntity>(_dbContext);
                _Repositories.Add(type, Repositoroy);
            }
            return _Repositories[type] as IGenericRepository<TEntity>;
        }
    }
}
