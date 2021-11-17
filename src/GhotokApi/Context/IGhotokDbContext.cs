using Ghotok.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using QQuery.Context;

namespace GhotokApi.Context
{
    public interface IGhotokDbContext:IQqContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<User> Users { get; set; }

    }
}