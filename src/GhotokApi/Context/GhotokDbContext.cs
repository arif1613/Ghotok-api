using Ghotok.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using QQuery.Context;

namespace GhotokApi.Context
{
    public class GhotokDbContext : QqContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=GhotokApiDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

    }
}
