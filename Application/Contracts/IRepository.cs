using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IRepository<T> where T : IMongoCommon
    {
        T Create();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(object id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        IQueryable<T> Collection { get; }
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate = null, bool includeDeleted = false);
    }
}