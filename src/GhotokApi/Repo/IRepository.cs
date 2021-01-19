using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GhotokApi.Repo
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(TEntity entityToDelete);
        void Delete(object id);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties, bool? isLookingForBride = false, int startIndex = 0,
            int chunkSize = 0);

        IEnumerable<TEntity> GetRecent(Expression<Func<TEntity, bool>> filter,string includeProperties, bool isLookingForBride);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, string cacheKey, string includeProperties=null);
        TEntity GetByID(object id);
        IQueryable<TEntity> GetWithRawSql(FormattableString query, params object[] parameters);
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
    }
}