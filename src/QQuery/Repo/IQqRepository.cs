using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QQuery.Repo
{
    public interface IQqRepository<TEntity> where TEntity : class
    {
        dynamic Get(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy=null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter);
        IQueryable<TEntity> GetWithRawSql(FormattableString query, params object[] parameters);

        void Insert(TEntity entity);
        void Insert(List<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);

    }
}