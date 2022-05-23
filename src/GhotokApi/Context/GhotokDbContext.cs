using Ghotok.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using QQuery.Context;

namespace GhotokApi.Context
{
    public class GhotokDbContext : QqContext
    {
        public GhotokDbContext(string DbConnectionString) : base(DbConnectionString)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
