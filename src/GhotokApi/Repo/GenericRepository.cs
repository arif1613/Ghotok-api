using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ghotok.Data;
using Ghotok.Data.DataModels;
using GhotokApi.Utils.Cache;
using GhotokApi.Utils.DbOperations;
using Microsoft.Extensions.Configuration;

namespace GhotokApi.Repo
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal GhotokDbContext context;
        internal DbSet<TEntity> dbSet;
        private readonly ICacheHelper _cacheHelper;
        private readonly IConfiguration _configuration;


        public GenericRepository(GhotokDbContext context, ICacheHelper cacheHelper, IConfiguration configuration)
        {
            this.context = context;
            _cacheHelper = cacheHelper;
            _configuration = configuration;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties, bool? isLookingForBride = false, int startIndex = 0, int chunkSize = 0)
        {

            var cacheKey = CreateCacheKey(typeof(TEntity), startIndex, chunkSize, isLookingForBride.ToString(), includeProperties);

            if (_cacheHelper.Exists(cacheKey).GetAwaiter().GetResult())
            {
                return _cacheHelper.Get<IEnumerable<TEntity>>(cacheKey).GetAwaiter().GetResult();
            }

            IQueryable<TEntity> query = dbSet;



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

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }


            if (query == null) return null;

            if (orderBy != null)
            {
                query = orderBy(query);
                if (query == null) return null;
            }

            //add to cache
            if (isLookingForBride != null)
            {
                if (query.Count() % Convert.ToInt32(_configuration["UserInfoCacheChunkSize"]) == 0)
                {
                    _cacheHelper.Add(query.ToList(), cacheKey, Convert.ToInt32(_configuration["AppUserCacheMinute"])).GetAwaiter().GetResult();
                    if (_cacheHelper.Exists(CreateRestCacheKey(typeof(TEntity), isLookingForBride.ToString())).GetAwaiter().GetResult())
                    {
                        _cacheHelper.Clear(CreateRestCacheKey(typeof(TEntity), isLookingForBride.ToString())).GetAwaiter().GetResult();
                    }
                }
            }

            return query.ToList() ?? null;
        }

        public IEnumerable<TEntity> GetRecent(Expression<Func<TEntity, bool>> filter,string includeProperties, bool isLookingForBride)
        {
            if (_cacheHelper.Exists(CreateRestCacheKey(typeof(TEntity), isLookingForBride.ToString())).GetAwaiter().GetResult())
            {
                return _cacheHelper.Get<IEnumerable<TEntity>>(CreateRestCacheKey(typeof(TEntity), isLookingForBride.ToString())).GetAwaiter().GetResult();
            }
            IQueryable<TEntity> query = dbSet;
            query = query.Where(filter);
            var totalRecords = query.CountAsync().GetAwaiter().GetResult();
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
            if (!_cacheHelper.Exists(CreateRestCacheKey(typeof(TEntity), isLookingForBride.ToString())).GetAwaiter().GetResult())
            {
                _cacheHelper.Add<IEnumerable<TEntity>>(query.ToList(),
                    CreateRestCacheKey(typeof(TEntity), isLookingForBride.ToString()),
                    Convert.ToInt32(_configuration["RecentUserCacheMinute"])).GetAwaiter().GetResult();
            }

            return query ?? null;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, string cacheKey, string includeProperties = null)
        {
            if (cacheKey != null)
            {
                if (await _cacheHelper.Exists(cacheKey))
                {
                    return await _cacheHelper.Get<TEntity>(cacheKey);
                }
            }

            IQueryable<TEntity> query = dbSet;
            query =  query.Where(filter);


            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (query == null) return null;
            var data = query.FirstOrDefault();

            if (data != null)
            {
                await _cacheHelper.Add(data, cacheKey, Convert.ToInt32(_configuration["AppUserCacheMinute"]));
            }
            return data ?? null;
        }

        private string CreateCacheKey(Type type, in int startIndex, in int chunkSize, in string isLookingForBride, in string includeProperties)
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

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public IQueryable<TEntity> GetWithRawSql(FormattableString query, params object[] parameters)
        {
            throw new NotImplementedException();
        }



        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}