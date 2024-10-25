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

        public PlaygroundSelfHierarchyDbContext(DbContextOptions<PlaygroundSelfHierarchyDbContext> options) : base(options)
        {

        }
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
