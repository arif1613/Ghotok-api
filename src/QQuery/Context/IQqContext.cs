using Microsoft.EntityFrameworkCore;

namespace QQuery.Context
{
    public interface IQqContext
    {
        EntityState GetEntityState<TEntity>(TEntity entity) where TEntity : class;
        DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class;
        void UpdateEntry<TEntity>(TEntity entity) where TEntity : class;
        void DeleteEntry<TEntity>(TEntity entity) where TEntity : class;
        void SaveDatabase();
        void DisposeDatabase();
    }
}