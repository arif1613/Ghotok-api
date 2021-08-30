using System.Linq;
using Ghotok.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace GhotokApi.Context
{
    public interface IGhotokDbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<User> Users { get; set; }

    }
}