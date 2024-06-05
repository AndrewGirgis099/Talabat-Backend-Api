using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            if(typeof(T)==typeof(Product))
            {
                return (IReadOnlyCollection<T>) await _dbContext.Set<Product>().Include(P=>P.Brand).Include(P=>P.Category).ToListAsync();
            }
           return await _dbContext.Set<T>().ToListAsync();
        }

       

        public async Task<T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Product))
            {
                return await _dbContext.Set<Product>().Where(P=>P.Id==id).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() as T;
            }

            return await _dbContext.Set<T>().FindAsync(id);  
        }

        public async Task<T?> GetWithSpecAsync(ISpecification<T> Spec)
        {
            return await SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllWithSpecAsync(ISpecification<T> Spec)
        {
            return await SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>() , Spec).ToListAsync();
        }

        public async Task<int> GetCountAsync(ISpecification<T> Spec)
        {
            return await SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec).CountAsync();
        }

        public async Task Add(T entity)
        {
           await  _dbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}
