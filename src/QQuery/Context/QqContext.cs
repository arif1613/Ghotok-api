using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QQuery.Context
{
    public abstract class QqContext : DbContext, IQqContext
    {

        public QqContext()
        {
            //Create database if not exist
            if (Database.CanConnect()) return;
            try
            {
                Database.EnsureCreated();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\MSSQLSERVER01;Database=GhotokApiDb_Guid;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<UserShortInfo>(entity =>
            //{
            //    entity.HasNoKey();

            //    entity.ToView("UserShortInfo");

            //    entity.Property(e => e.Id).HasColumnName("Id");
            //    entity.Property(e => e.ContactNumber).HasColumnName("ContactNumber");
            //    entity.Property(e => e.Dob).HasColumnName("Dob");
            //    entity.Property(e => e.IsPictureUploaded).HasColumnName("IsPictureUploaded");
            //    entity.Property(e => e.LookingForBride).HasColumnName("LookingForBride");
            //    entity.Property(e => e.MaritalStatus).HasColumnName("MaritalStatus");
            //    entity.Property(e => e.Name).HasColumnName("Name");
            //    entity.Property(e => e.PictureName).HasColumnName("PictureName");
            //    entity.Property(e => e.email).HasColumnName("email");
            //});
        }
        public EntityState GetEntityState<TEntity>(TEntity entity) where TEntity : class
        {
            return Entry(entity).State;

        }

        public IQueryable<TEntity> GetQuerybleDbSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public void UpdateEntry<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void DeleteEntry<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Deleted;

        }

        public void SaveDatabase()
        {
            SaveChanges();

        }

        public void DisposeDatabase()
        {
            Dispose();
        }

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }
    }
}
