using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDo_Auth_DAL.Models;

namespace ToDo_Auth_DAL.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<AuthToDoUserEntity>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<AuthToDoUserEntity> AppUsers { get; set; }
    }
}
