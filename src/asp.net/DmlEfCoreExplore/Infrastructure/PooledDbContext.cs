using Bogus;
using Microsoft.EntityFrameworkCore;

namespace DmlEfCoreExplore.Infrastructure
{
    public class PooledDbContext : DbContext
    {
        public string Id { get; private set; }
        public Guid Guid { get; private set; }
        public DateTime Created { get; private set; }

        public DbSet<PooledContextEntity01> Entity01s { get; set; }
        public DbSet<PooledContextEntity02> Entity02s { get; set; }

        public PooledDbContext(DbContextOptions<PooledDbContext> options) : base(options)
        {
            this.Id = new Faker().Name.FullName();
            this.Guid = Guid.NewGuid();
            Created = DateTime.Now;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PooledContextEntity01>(
            x =>
            {
                x.HasKey(x => x.Id);
                x.HasData(new PooledContextEntity01()
                {
                    Id = 1,
                    Name = new Faker().Name.FullName(),
                });
            });

            modelBuilder.Entity<PooledContextEntity02>(
            x =>
            {
                x.HasKey(x => x.Id);
                x.HasData(new PooledContextEntity02()
                {
                    Id = 101,
                    Name = new Faker().Name.FullName(),
                });
            });

        }
    }

    public class PooledContextEntity01
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class PooledContextEntity02
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
