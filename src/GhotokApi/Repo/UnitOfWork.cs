using System;
using Ghotok.Data;
using Ghotok.Data.DataModels;
using GhotokApi.Utils.Cache;
using Microsoft.Extensions.Configuration;

namespace GhotokApi.Repo
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private GhotokDbContext _dbContext;
        private GenericRepository<AppUser> _appusers;
        private GenericRepository<User> _users;
        private readonly ICacheHelper _cacheHelper;
        private readonly IConfiguration _configuration;
        public UnitOfWork(GhotokDbContext dbContext, ICacheHelper cacheHelper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _cacheHelper = cacheHelper;
            _configuration = configuration;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<AppUser> AppUseRepository
        {
            get
            {
                return _appusers ??= new GenericRepository<AppUser>(_dbContext,_cacheHelper,_configuration);
            }
        }
        public IRepository<User> UserRepository {
            get
            {
                return _users ??= new GenericRepository<User>(_dbContext, _cacheHelper, _configuration);
            }
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}