using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery , ISpecification<TEntity> Spec) 
        {
            var query = InputQuery;  // _dbContext.set<TEntity>()    TEntity = Employee

            if (Spec.Criteria is not null)  // E => E.Id == 1
            {
                query= query.Where(Spec.Criteria);
            }

            if(Spec.OrderBy is not null)
            {
                query = query.OrderBy(Spec.OrderBy);
            }

            if(Spec.OrderByDesc is not null)
            {
                query = query.OrderByDescending(Spec.OrderByDesc);
            }

            if(Spec.IsPaginationEnabled)
            {
                query = query.Skip(Spec.Skip).Take(Spec.Take);  
            }

            query = Spec.Inculeds.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            return query;
        }
    }
}
