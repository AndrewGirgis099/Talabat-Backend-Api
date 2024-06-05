using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
       Task<T?> GetAsync(int id);
       Task<IReadOnlyCollection<T>> GetAllAsync();

        Task<IReadOnlyCollection<T>> GetAllWithSpecAsync(ISpecification<T> Spec);
        Task<T?> GetWithSpecAsync(ISpecification<T> Spec);

        Task<int> GetCountAsync(ISpecification<T> Spec);

        Task Add (T entity);

        void Delete (T entity); 
        void Update (T entity);
    }
}
