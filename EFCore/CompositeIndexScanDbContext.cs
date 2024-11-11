using System;
using Microsoft.EntityFrameworkCore;

namespace IziHardGames.Playgrounds.ForEfCore
{
    public class CompositeIndexScanDbContext : DbContext
    {
        public DbSet<EntityCompositeIndex> EntityiesWithCompositeIndex { get; set; }

        public CompositeIndexScanDbContext(DbContextOptions<CompositeIndexScanDbContext> opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public void Init()
        {
            for (int i = 1; i < 2000; i++)
            {
                var e = new EntityCompositeIndex()
                {
                    Id = i,
                    GuidFirst = Guid.NewGuid(),
                    GuidSecond = Guid.NewGuid(),
                    Type = (ECompositeIndexType)(i % 4),
                };
                EntityiesWithCompositeIndex.Add(e);
            }
        }
    }

    public class EntityCompositeIndex
    {
        public int Id { get; set; }
        public Guid? GuidFirst { get; set; }
        public Guid? GuidSecond { get; set; }
        public ECompositeIndexType? Type { get; set; }
    }

    public enum ECompositeIndexType
    {
        None,
        Some,
        All,
        Any,
    }
}
