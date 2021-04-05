using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ghotok.Data.Context;
using Ghotok.Data.Utils.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;

namespace Ghotok.Data.Repo
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IGhotokDbContext context;
        private readonly ICacheHelper _cacheHelper;
        private readonly IConfiguration _configuration;


        public GenericRepository(IGhotokDbContext context, ICacheHelper cacheHelper, IConfiguration configuration)
        {
            this.context = context;
            _cacheHelper = cacheHelper;
            _configuration = configuration;
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool? isLookingForBride=false,
            int startIndex = 0, int chunkSize = 0, bool disableTracking = true)
        {
            string cacheKey = null;
            if (include!=null)
            {
                cacheKey = CreateCacheKey(typeof(TEntity), startIndex, chunkSize, isLookingForBride.ToString(), IncludeProperties.AppUserIncludingAllProperties);
            }
            else
            {
                cacheKey = CreateCacheKey(typeof(TEntity), startIndex, chunkSize, isLookingForBride.ToString(), null);
            }

            if (_cacheHelper.Exists(cacheKey))
            {
                return _cacheHelper.Get<IEnumerable<TEntity>>(cacheKey);
            }

            IQueryable<TEntity> query = context.GetDbSet<TEntity>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                if (startIndex == 0 && chunkSize == 0)
                {
                    query = query.Where(filter);
                }
                else
                {
                    query = query.Where(filter).Skip(startIndex).Take(chunkSize);
                }
            }

           

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
                if (query == null) return null;
            }


           

            if (query == null) return null;

            

            //add to cache
            if (isLookingForBride != null)
            {
                if (query.Count() % Convert.ToInt32(_configuration["UserInfoCacheChunkSize"]) == 0)
                {
                    _cacheHelper.Add(query.ToList(), cacheKey, Convert.ToInt32(_configuration["AppUserCacheMinute"]));
                    if (_cacheHelper.Exists(CreateRestCacheKey(typeof(TEntity), isLookingForBride.ToString())))
                    {
                        _cacheHelper.Clear(CreateRestCacheKey(typeof(TEntity), isLookingForBride.ToString()));
                    }
                }
            }

            return query.ToList() ?? null;




        }


        public IEnumerable<TEntity> GetRecent(Expression<Func<TEntity, bool>> filter,string includeProperties, bool isLookingForBride)
        {
            if (_cacheHelper.Exists(CreateRestCacheKey(typeof(TEntity), isLookingForBride.ToString())))
            {
                return _cacheHelper.Get<IEnumerable<TEntity>>(CreateRestCacheKey(typeof(TEntity), isLookingForBride.ToString()));
            }
            IQueryable<TEntity> query = context.GetDbSet<TEntity>();

            query = query.Where(filter);
            var totalRecords = query.Count();
            var recentRecordsWithoutCahing = totalRecords % Convert.ToInt32(_configuration["UserInfoCacheChunkSize"]);
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
            if (!_cacheHelper.Exists(CreateRestCacheKey(typeof(TEntity), isLookingForBride.ToString())))
            {
                _cacheHelper.Add<IEnumerable<TEntity>>(query.ToList(),
                    CreateRestCacheKey(typeof(TEntity), isLookingForBride.ToString()),
                    Convert.ToInt32(_configuration["RecentUserCacheMinute"]));
            }

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