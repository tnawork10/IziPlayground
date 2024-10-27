using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace IziHardGames.Playgrounds.ForEfCore
{
    public class PlaygroundSelfHierarchyDbContext : DbContext
    {
        public DbSet<EntityHierarchy> Hierarchies { get; set; }
        public DbSet<EntityHierarchyLTree> LTrees { get; set; }

        public PlaygroundSelfHierarchyDbContext(DbContextOptions<PlaygroundSelfHierarchyDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EntityHierarchyLTree>().Property(x => x.Path).HasColumnType("ltree");
            modelBuilder.Entity<EntityHierarchyLTree>().HasOne(x => x.Parent).WithMany(x => x.Childs).HasForeignKey(x => x.ParentGuid);
        }
    }


    public class EntityHierarchyLTree
    {
        [Key]
        public Guid Guid { get; set; } = Guid.NewGuid();
        [JsonIgnore] public LTree Path { get; set; }
        public Guid? ParentGuid { get; set; }
        public EntityHierarchyLTree? Parent { get; set; }
        [JsonIgnore] public ICollection<EntityHierarchyLTree>? Childs { get; set; }
    }

    public class EntityHierarchy
    {
        [Key]
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Guid? ParentGuid { get; set; }
        [JsonIgnore] public EntityHierarchy? Parent { get; set; }
        public ICollection<EntityHierarchy>? Childs { get; set; }
    }

    public class EntityHierarchyWithPostgresHierarchyId
    {

    }
}
