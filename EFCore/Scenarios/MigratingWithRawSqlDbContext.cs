using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IziHardGames.Playgrounds.ForEfCore.Scenarios
{
    public class MigratingWithRawSqlDbContext(DbContextOptions<MigratingWithRawSqlDbContext> options) : DbContext(options)
    {
        public DbSet<Entity01> Entities01 { get; set; }
        public DbSet<Entity02> Entities02 { get; set; }
        public DbSet<Entity03> Entities03 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entity01>(x =>
            {
                x.HasKey(x => x.Id);
                x.HasOne(x => x.Entity03).WithMany(x => x.Entities01).HasForeignKey(x => x.Entity03Id);
            });
            modelBuilder.Entity<Entity02>(x =>
            {
                x.HasKey(x => x.Id);
            });
            modelBuilder.Entity<Entity03>(x =>
            {
                x.HasKey(x => x.Id);
                x.HasIndex(x => new { x.Guid0, x.Guid1, x.Int0 }).IsUnique();
            });
        }

        public async Task<int> InitAsync()
        {
            for (int i = 0; i < 100; i++)
            {
                var e1 = new Entity01()
                {
                    Id = 0,
                    Guid0 = Guid.NewGuid(),
                    Guid1 = Guid.NewGuid(),
                    Int0 = i % 4,
                };
                Entities01.Add(e1);
            }
            return await SaveChangesAsync();
        }

        public class Entity03
        {
            public int Id { get; set; }
            public Guid Guid0 { get; set; }
            public Guid Guid1 { get; set; }
            public int Int0 { get; set; }
            public ICollection<Entity01> Entities01 { get; set; } = null!;
        }

        public class Entity02
        {
            public int Id { get; set; }

        }

        public class Entity01
        {
            public int Id { get; set; }
            public Guid Guid0 { get; set; }
            public Guid Guid1 { get; set; }
            public int Int0 { get; set; }

            public int? Entity03Id { get; set; }
            public Entity03? Entity03 { get; set; }
        }
    }
}
