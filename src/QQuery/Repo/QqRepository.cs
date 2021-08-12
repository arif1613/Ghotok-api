using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using QQuery.Context;

namespace QQuery.Repo
{
    public class QqRepository<TEntity> : IQqRepository<TEntity> where TEntity : class
    {
        private readonly IQqContext context;
        public QqRepository(IQqContext context)
        {
            this.context = context;
        }

        public dynamic Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = false)
        {

            IQueryable<TEntity> query = context.GetDbSet<TEntity>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (include != null)
            {
                query = include(query);
            }

            if (orderBy == null) return query;
            query = orderBy(query);
            return query;
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = context.GetDbSet<TEntity>();
            query = query.AsNoTracking();
            query = query.Where(filter);
            return query.ToList();
        }


        public IQueryable<TEntity> GetWithRawSql(FormattableString query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public virtual void Insert(TEntity entity)
        {
            try
            {
                if (context.GetEntityState(entity) != EntityState.Detached)
                {
                    context.GetDbSet<TEntity>().Attach(entity);
                }
                context.GetDbSet<TEntity>().Add(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public void Insert(List<TEntity> entities)
        {
            try
            {
                if (context.GetEntityState(entities) == EntityState.Detached)
                {
                    context.GetDbSet<TEntity>().AttachRange(entities);
                }
                context.GetDbSet<TEntity>().AddRange(entities);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public void Update(TEntity entity)
        {
            context.GetDbSet<TEntity>().Attach(entity);
            context.UpdateEntry(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            context.DeleteEntry(entity);
        }


        #region Private methods

        private string CreateCacheKey(Type type, in int startIndex, in int chunkSize, in string isLookingForBride, string includeProperties)
        {
            var cacheKey = $"{type.Name}_{startIndex}_{chunkSize}_{isLookingForBride}";
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    cacheKey = $"{cacheKey}_{includeProperty}";
                }
            }

            return cacheKey;
        }
        private string CreateRestCacheKey(Type type, in string isLookingForBride)
        {
            return $"{type.Name}_Other1_{isLookingForBride}";
        }

        #endregion
    }
}