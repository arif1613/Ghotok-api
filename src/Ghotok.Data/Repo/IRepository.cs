using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace Ghotok.Data.Repo
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(IEnumerable<Expression<Func<TEntity, bool>>> filters,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy=null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
             int startIndex = 0, int chunkSize = 0, bool disableTracking = true);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter);
        dynamic GetRecent(IEnumerable<Expression<Func<TEntity, bool>>> filters, string includeProperties);
        IQueryable<TEntity> GetWithRawSql(FormattableString query, params object[] parameters);

        void Insert(TEntity entity);
        void Insert(List<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);

    }
}