using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace IziHardGames.Playgrounds.ForEfCore
{
    public class DmlResearchContext : DbContext
    {
        public DbSet<DmlEntity> Entities { get; set; }
        public DbSet<DmlCollectionNavPropParent> CollectionNavParents { get; set; }
        public DbSet<DmlCollectionNavPropChild> CollectionNavChilds { get; set; }
        public DbSet<DmlUpdate> Updates { get; set; }
        public DmlResearchContext(DbContextOptions<DmlResearchContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DmlCollectionNavPropChild>()
                        .HasOne(x => x.Parent)
                        .WithMany(x => x.Childs)
                        .HasForeignKey(x => x.ParentId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class DmlEntity
    {
        public Guid Id { get; set; }
        public string TextField { get; set; } = "Some";
        public double DoubleField { get; set; }
        public int IntField { get; set; }
    }

    public class DmlCollectionNavPropParent
    {
        public int Id { get; set; }
        public ICollection<DmlCollectionNavPropChild>? Childs { get; set; }
    }
    public class DmlCollectionNavPropChild
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        [JsonIgnore] public DmlCollectionNavPropParent Parent { get; set; }
    }

    public class DmlUpdate
    {
        public int Id { get; set; }
        public double ValueAsDouble { get; set; }
        public string ValueAsString { get; set; }
        public int ValueAsInt { get; set; }
    }

}
