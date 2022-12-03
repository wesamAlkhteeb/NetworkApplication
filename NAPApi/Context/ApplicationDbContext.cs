using Microsoft.EntityFrameworkCore;
using NAPApi.Entity;

namespace NAPApi.Context
{
    public class ApplicationDbContext:DbContext
    {

        public DbSet<User> users { get; set; }
        public DbSet<Logging> loggings { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Group> groups { get; set; }
        public DbSet<PermessionsGroup> permessionsGroups { get; set; }
        public DbSet<Files> files { get; set; }
        public DbSet<Report> reports { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option):base(option)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
