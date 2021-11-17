using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QQuery.Repo
{
    public interface IQuickQueryRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get(IEnumerable<Expression<Func<TEntity, bool>>> filters=null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int startIndex = 0, int chunkSize = 0, bool disableTracking = true);

        IQueryable<TEntity> Get(IEnumerable<Expression<Func<TEntity, bool>>> filters,
            Expression<Func<TEntity, bool>> orderBy, Expression<Func<TEntity, bool>> include);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetRecent(IEnumerable<Expression<Func<TEntity, bool>>> filters, string includeProperties);
        IQueryable<TEntity> GetWithRawSql(FormattableString query, params object[] parameters);

        void Insert(TEntity entity);
        void Insert(List<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);

    }
}