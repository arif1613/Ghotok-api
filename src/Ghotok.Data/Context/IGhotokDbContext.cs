using System;
using Ghotok.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Ghotok.Data.Context
{
    public interface IGhotokDbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<User> Users { get; set; }

        EntityState GetEntityState<TEntity>(TEntity entity) where TEntity : class;
        void UpdateEntry<TEntity>(TEntity entity) where TEntity : class;
        void DeleteEntry<TEntity>(TEntity entity) where TEntity : class;
        void SaveDatabase();
        void DisposeDatabase();

        DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class;
    }
}