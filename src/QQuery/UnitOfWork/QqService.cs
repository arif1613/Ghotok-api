using System;
using QQuery.Context;
using QQuery.Repo;

namespace QQuery.UnitOfWork
{
    public class QqService<TEntity> :IDisposable, IQqService<TEntity> where TEntity : class
    {

        private IQqContext _dbContext;
        private QuickQueryRepository<TEntity> _dataRepo;
        public QqService(IQqContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQuickQueryRepository<TEntity> QqRepository
        {
            get
            {
                return _dataRepo ??= new QuickQueryRepository<TEntity>(_dbContext);
            }
        }
        public void Commit()
        {
            _dbContext.SaveDatabase();
        }

        #region Dispose pattern


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.DisposeDatabase();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}