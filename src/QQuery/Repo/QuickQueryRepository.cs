using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using QQuery.Context;
using QQuery.Helper;

namespace QQuery.Repo
{
    public class QuickQueryRepository<TEntity> : IQuickQueryRepository<TEntity> where TEntity : class
    {
        private readonly IQqContext _context;


        public QuickQueryRepository(IQqContext context)
        {
            _context = context;
        }


        public IQueryable<TEntity> Get(IEnumerable<Expression<Func<TEntity, bool>>> filters=null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int startIndex = 0, int chunkSize = 0, bool disableTracking = true)
        {



            IQueryable<TEntity> query = _context.GetQuerybleDbSet<TEntity>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (filters != null && filters.Any())
            {
                query = query.AndAll(filters);
            }

            if (startIndex != 0 || chunkSize != 0)
            {
                query = query.Skip(startIndex).Take(chunkSize);
            }

            if (include != null && query != null)
            {
                query = include(query);
            }

            if (orderBy != null && query != null)
            {
                query = orderBy(query);
            }

            return query ?? null;
            
        }

        public IQueryable<TEntity> Get(IEnumerable<Expression<Func<TEntity, bool>>> filters, Expression<Func<TEntity, bool>> orderBy, Expression<Func<TEntity, bool>> include)
        {
            var translator = new QueryTranslator();
            string whereClause = translator.Translate(filters.FirstOrDefault());
            string orderbyClause = translator.Translate(orderBy);

            var f = whereClause + " " + orderbyClause;
            IQueryable<TEntity> query = _context.GetQuerybleDbSet<TEntity>();
            return query;
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = _context.GetQuerybleDbSet<TEntity>();
            query = query.AsNoTracking();
            query = query.Where(filter);
            return query.ToList();
        }


        public IEnumerable<TEntity> GetRecent(IEnumerable<Expression<Func<TEntity, bool>>> filters, string includeProperties)
        {
            //if (_cacheHelper.Exists(CreateRestCacheKey(typeof(TEntity), filters.ToString())))
            //{
            //    return _cacheHelper.Get<IEnumerable<TEntity>>(CreateRestCacheKey(typeof(TEntity), filters.ToString()));
            //}
            IQueryable<TEntity> query = _context.GetQuerybleDbSet<TEntity>();

            query = query.AndAll(filters);
            var totalRecords = query.Count();
            //var recentRecordsWithoutCahing = totalRecords % Convert.ToInt32(_configuration["UserInfoCacheChunkSize"]);
            var recentRecordsWithoutCahing = totalRecords % 20;

            var skip = (totalRecords - recentRecordsWithoutCahing);
            var take = (totalRecords - skip);
            if (take == 0) return null;

            query = query.Skip(skip).Take(take);

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (query == null) return null;

            //add to cache
            //if (!_cacheHelper.Exists(CreateRestCacheKey(typeof(TEntity), filters.ToString())))
            //{
            //    _cacheHelper.Add<IEnumerable<TEntity>>(query.ToList(),
            //        CreateRestCacheKey(typeof(TEntity), filters.ToString()),
            //        Convert.ToInt32(_configuration["RecentUserCacheMinute"]));
            //}

            return query.ToList() ?? null;
        }

        public IQueryable<TEntity> GetWithRawSql(FormattableString query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public virtual void Insert(TEntity entity)
        {
            try
            {
                if (_context.GetEntityState(entity) != EntityState.Detached)
                {
                    _context.GetDbSet<TEntity>().Attach(entity);
                }
                _context.GetDbSet<TEntity>().Add(entity);
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
                if (_context.GetEntityState(entities) == EntityState.Detached)
                {
                    _context.GetDbSet<TEntity>().AttachRange(entities);
                }
                _context.GetDbSet<TEntity>().AddRange(entities);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public void Update(TEntity entity)
        {
            _context.GetDbSet<TEntity>().Attach(entity);
            _context.UpdateEntry(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _context.DeleteEntry(entity);
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