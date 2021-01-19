using Ghotok.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Ghotok.Data
{
    public interface IGhotokDbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<User> Users { get; set; }
    }
}