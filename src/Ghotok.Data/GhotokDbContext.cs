using Microsoft.EntityFrameworkCore;
using Ghotok.Data.DataModels;

namespace Ghotok.Data
{
    public class GhotokDbContext : DbContext, IGhotokDbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public GhotokDbContext(DbContextOptions<GhotokDbContext> options) : base(options)
        {
        }
    }
}
