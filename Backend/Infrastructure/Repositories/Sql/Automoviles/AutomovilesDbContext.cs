using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.Automoviles
{
    public class AutomovilesDbContext : DbContext
    {
        public AutomovilesDbContext(DbContextOptions<AutomovilesDbContext> options) : base(options) { }

        public DbSet<Automovil> Automoviles => Set<Automovil>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.AutomovilConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}